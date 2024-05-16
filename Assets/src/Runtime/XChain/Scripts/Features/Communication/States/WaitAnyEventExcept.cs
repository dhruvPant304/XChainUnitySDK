using System.Threading;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    [NodeTint("#880808")]
    public class WaitAnyEventExcept : State
    {
        [Output] public NodePort exit;
        [SerializeField] private XChainEvents excludeEvent;
        [SerializeField] private bool consumeEvent = true;
        protected override void Enter()
        {
            WaitEvent(_stateCancellationtoken);
        }

        protected override void Exit()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            // new System.NotImplementedException();
        }

        private async UniTask WaitEvent(CancellationToken token)
        {
            await XChain.WaitAnyEventExcept(excludeEvent, consumeEvent, token);
            ExitThroughNodePort("exit");
        }
    }
}