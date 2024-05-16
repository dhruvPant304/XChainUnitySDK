using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using XNode;
using XNodeStateMachine.NodePorts;

#if ZENJECT_INJECT_IN_STATES
using Zenject;
#endif


namespace XNodeStateMachine
{
	[CreateAssetMenu]
	public class StateMachineGraph : NodeGraph
	{
#if ZENJECT_INJECT_IN_STATES
		private DiContainer _diContainer;
#endif
		private bool _running;
		private State _start;
		private CancellationTokenSource _stateCTS;

		public string exitName;

#if ZENJECT_INJECT_IN_STATES
		private void Init(DiContainer container = null)
		{
			_diContainer = container;
			foreach (var node in nodes)
			{
				if (node is not State state) continue;
				state.ResetState();
				if(container != null) state.InjectUsingContainer(container);
				if (state is not StartState startState) continue;
				_start = startState;
			}
		}
#else
		private void Init()
		{
			foreach (var node in nodes)
			{
				if (node is not State state) continue;
				state.ResetState();
				if (state is not StartState startState) continue;
				_start = startState;
			}
		}
#endif

#if ZENJECT_INJECT_IN_STATES
		public async UniTask RunStateMachine(DiContainer container = null)
		{
			_running = true;
			_stateCTS = new CancellationTokenSource();
			Init(container);
			RunBranch(_start);
			await UniTask.WaitWhile(() => _running);
		}
#else
		public async UniTask RunStateMachine()
		{
			_running = true;
			_stateCTS = new CancellationTokenSource();
			Init();
			RunBranch(_start);
			await UniTask.WaitWhile(() => _running);
		}
#endif

		private async void RunBranch(State state)
		{
			var current = state;
			while (current is not ExitState && current is not Parallel && _running && current != null)
			{
				var next = await current.RunState(_stateCTS.Token); 
				current = next;
			}
			if(!_running) return;
			switch (current)
			{
				case Parallel parallel:
				{
					foreach (var nodePort in parallel.DynamicOutputs)
					{
						if (!nodePort.IsConnected) continue;
						if (nodePort.Connection.node is State branchEntry)
						{
							RunBranch(branchEntry);
						}
					}
					break;
				}
				case ExitState exitState:
					_running = false;
					exitName = exitState.exitName;
					_stateCTS.Cancel();
					_stateCTS.Dispose();
					break;
			}
		}
	}
}