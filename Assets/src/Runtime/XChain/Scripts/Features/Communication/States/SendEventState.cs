using Assets.Scripts.Features.Communication.Enums;
using Assets.Scripts.Features.Communication.Messages;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using RiskyTools.Messaging.Interfaces;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    [NodeTint("#3498db")]
    public class SendEventState : State
    {
        [Output] public NodePort exit;
        [SerializeField] private XChainEvents eventType;
        protected override void Enter()
        {
            var message = new XChainEventMessage(eventType);
            XChain.Instance.MessagingService.Publish(this,message);
            ExitThroughNodePort("exit");
        }

        protected override void Exit()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }
    }
}