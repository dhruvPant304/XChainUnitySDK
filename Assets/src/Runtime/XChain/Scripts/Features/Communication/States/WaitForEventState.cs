using System.Threading;
using Assets.Scripts.Features.Communication.Enums;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    [NodeTint("#e67e22")]
    public class WaitForEventState : State
    {
        [Output] public NodePort exit;
        [SerializeField] private XChainEvents eventType;
        [SerializeField] private bool consumeEvent = true;
        
        protected override void Enter()
        {
            WaitChainEvent(_stateCancellationtoken);
        }

        protected override void Exit()
        {
            
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        private async UniTask WaitChainEvent(CancellationToken token)
        {
            await XChain.WaitXChainEvent(eventType, consumeEvent, token);
            ExitThroughNodePort("exit");
        }
    }
}