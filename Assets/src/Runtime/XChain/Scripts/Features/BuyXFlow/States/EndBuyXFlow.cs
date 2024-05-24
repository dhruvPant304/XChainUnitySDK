using Features.BuyXFlow.UIControllers;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States {
    public class EndBuyXFlow : State {
        [Output] public NodePort exit;

        protected override void Enter() {
            XChainCanvas.Instance.HideBuyXView();
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

