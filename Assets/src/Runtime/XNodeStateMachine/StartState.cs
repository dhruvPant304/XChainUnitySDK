using UnityEngine;
using XNode;

namespace XNodeStateMachine
{
	[NodeTint("#008000")]
	public class StartState : State
	{
		[Output] public NodePort exit;

		protected override void Enter()
		{
			ExitThroughNodePort("exit");
		}

		protected override void Exit()
		{
		}

		protected override void UpdateState()
		{
		}
	}
}