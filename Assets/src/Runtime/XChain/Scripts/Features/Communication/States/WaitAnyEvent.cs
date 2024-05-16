using System.Threading;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Communication.States
{
    [NodeTint("#ffc300")]
    public class WaitAnyEvent : State
    {
        [Output] public NodePort exit;
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
            await XChain.WaitAnyEvent(consumeEvent, token);
            ExitThroughNodePort("exit");
        }
    }
}