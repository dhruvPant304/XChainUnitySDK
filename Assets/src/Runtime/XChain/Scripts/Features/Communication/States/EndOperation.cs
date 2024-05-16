using Assets.Scripts.Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    public class EndOperation: State
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