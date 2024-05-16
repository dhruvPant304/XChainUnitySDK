using Cysharp.Threading.Tasks;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace XNodeStateMachine
{
    [NodeTint("#800080")]
    public class SubStateMachineState : State
    {
        [SerializeField] public StateMachineGraph subState;

        private bool _running;

        protected override void Enter()
        {
            _running = true;
            RunSubState();
        }

        protected override void Exit()
        {
            
        }

        protected override void UpdateState()
        {
            if (!_running)
            {
                string exitName = subState.exitName;
                if (string.IsNullOrWhiteSpace(exitName)) exitName = "exit";
                ExitThroughNodePort(exitName);
            }
        }

        private async void RunSubState()
        {
#if ZENJECT_INJECT_IN_STATES
            await subState.RunStateMachine(_container);
#else
            await subState.RunStateMachine();
#endif
            _running = false;
        }
    }
}
