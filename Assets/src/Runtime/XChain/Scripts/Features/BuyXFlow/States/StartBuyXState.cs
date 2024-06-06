using Features.BuyXFlow.UIControllers;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States
{
    public class StartBuyXState : State {
        [Output] public NodePort exit;

        protected override void Enter() {
            SendRequestAsync();
            ExitThroughNodePort("exit");
        }

        private void SendRequestAsync() {
            XChainCanvas.Instance.OpenBuyXView(); 
        }

        protected override void Exit(){
    
        }

        protected override void UpdateState() {
            
        }
    }
}
