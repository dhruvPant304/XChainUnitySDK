using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using XNode;
using XNodeStateMachine.NodePorts;

#if ZENJECT_INJECT_IN_STATES
using Zenject;
#endif


namespace XNodeStateMachine
{
	public abstract class State : Node
	{
#if ZENJECT_INJECT_IN_STATES
		protected DiContainer _container;
		public void InjectUsingContainer(DiContainer container)
		{
			container.Inject(this);
			_container = container;
		}
#endif
		[Input] public NodePort enter;
		protected abstract void Enter();
		protected abstract void Exit();
		protected abstract void UpdateState();
		private NodePort _stateExitNode;
		public NodePort ExitedFrom => _stateExitNode;
		protected void ExitThroughNodePort(string exitNode) => _stateExitNode = GetOutputPort(exitNode);
		protected CancellationToken _stateCancellationtoken;
		private bool _isCompleted;
		public bool isCompleted => _isCompleted;
		private bool _isRunning;
		public bool isRunning => _isRunning;
		public void ResetState()
		{
			_isRunning = false;
			_isCompleted = false;
		}
		public async UniTask<State> RunState(CancellationToken token = new CancellationToken())
		{
			_isRunning = true;
			_isCompleted = false;
			_stateCancellationtoken = token;
			if (token.IsCancellationRequested) return null;
			_stateExitNode = null;
			Enter();
			while(_stateExitNode == null && !token.IsCancellationRequested)
			{
				UpdateState();
				await UniTask.Yield();
			}
			Exit();
			_isRunning = false;
			_isCompleted = true;
			if (token.IsCancellationRequested) return null;
			if (_stateExitNode == null) return null;
			if (_stateExitNode.Connection == null) return null;
			if (_stateExitNode.Connection.node == null) return null;
			return _stateExitNode.Connection.node as State;
		}

		private void OnDestroy()
		{
			ResetState();
		}
	}
}