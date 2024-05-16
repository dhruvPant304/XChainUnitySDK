using Features.Web3Authentication.StateMachines.Interfaces;
using Features.Web3Authentication.UIControllers;
using XNode;
using XNodeStateMachine;

namespace Features.Web3Authentication.StateMachines.States
{
    public class LandingPageState : State
    {
        [Output] public NodePort signUp;
        [Output] public NodePort login;
        
        //Dependencies
        private LandingPageUIController _landingPageUIController;
        private IWeb3AuthFlowBlackBoard _web3AuthFlowBlackBoard;
        private NavigationUIController _navigationUIController;

        
        public void Inject(LandingPageUIController landingPageUIController,
            IWeb3AuthFlowBlackBoard web3AuthFlowBlackBoard,
            NavigationUIController navigationUIController)
        {
            _landingPageUIController = landingPageUIController;
            _web3AuthFlowBlackBoard = web3AuthFlowBlackBoard;
            _navigationUIController = navigationUIController;
        }
        
        protected override void Enter()
        {
            _landingPageUIController.onLoginButtonClicked += OnLoginClicked;
            _landingPageUIController.onSignUpButtonClicked += OnSignUpClicked;
            _landingPageUIController.Show();
            _navigationUIController.Show();
            _navigationUIController.HideBackButton();
        }

        protected override void Exit()
        {
            _landingPageUIController.onLoginButtonClicked -= OnLoginClicked;
            _landingPageUIController.onSignUpButtonClicked -= OnSignUpClicked;
            _landingPageUIController.Hide();
            _navigationUIController.ShowBackButton();
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        private void OnLoginClicked()
        {
            ExitThroughNodePort("login");
        }

        private void OnSignUpClicked()
        {
            ExitThroughNodePort("signUp");
        }
    }
}