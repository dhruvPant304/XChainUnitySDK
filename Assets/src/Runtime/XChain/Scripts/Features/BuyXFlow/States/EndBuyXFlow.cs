using Features.BuyXFlow.UIControllers;
using Features.Communication.Singletons;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States {
    public class EndBuyXFlow : State {
        [Output] public NodePort exit;

        protected override async void Enter() {
            XChainCanvas.Instance.HideBuyXView();
            await XChain.FetchXTokenBalance();
            ExitThroughNodePort("exit");
        }

        protected override void Exit() {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState() {
            //throw new System.NotImplementedException();
        }
    }
}

