using UnityEngine;

namespace XNodeStateMachine
{
	[NodeTint("#ff0000")]
	public class ExitState : State
	{
		[SerializeField] public string exitName;
		protected override void Enter()
		{
		}

		protected override void Exit()
		{
		}

		protected override void UpdateState()
		{
		}
	}
}