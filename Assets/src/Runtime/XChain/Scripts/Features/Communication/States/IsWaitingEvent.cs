using Assets.Scripts.Features.Communication.Enums;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    [NodeTint("#8a2be2")]
    public class IsWaitingEvent : State
    {
        [Output] public NodePort yes;
        [Output] public NodePort no;
        [SerializeField] private XChainEvents eventType;
        
        protected override void Enter()
        {
            if (XChain.IsEventWaiting(eventType))
            {
                ExitThroughNodePort("yes");
            }
            else
            {
                ExitThroughNodePort("no");
            }
        }

        protected override void Exit()
        {
            
        }

        protected override void UpdateState()
        {
            
        }
    }
}