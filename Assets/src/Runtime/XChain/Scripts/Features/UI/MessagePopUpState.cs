using Features.Web3Authentication.StateMachines.Interfaces;
using Features.Web3Authentication.UIControllers;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.UI
{
    [NodeWidth(250)]
    [NodeTint("#964b00")]
    public class MessagePopUpState : State
    {
        [Output] public NodePort okay;
        [Output] public NodePort cancel;

        [SerializeField] public string bodyMessage;
        [SerializeField] public string headerMessage;
        [SerializeField] public string confirmationButtonText;
        [SerializeField] public string declineButtonText;

        //Dependencies
        private MessagePopUpUIController _messagePopUpUIController;
        
        public void Inject(MessagePopUpUIController messagePopUpUIController)
        {
            _messagePopUpUIController = messagePopUpUIController;
        }

        protected override void Enter()
        {
            _messagePopUpUIController.bodyText.text = bodyMessage;
            _messagePopUpUIController.headerText.text = headerMessage;
            _messagePopUpUIController.confirmationButtonText.text = confirmationButtonText;
            _messagePopUpUIController.declineButtonText.text = declineButtonText;
            _messagePopUpUIController.onOkayButtonClicked = OnOkayClicked;
            _messagePopUpUIController.onCancelButtonClicked = OnCancelClicked;
            _messagePopUpUIController.Show();
        }

        protected override void Exit()
        {
            _messagePopUpUIController.bodyText.text = "";
            _messagePopUpUIController.headerText.text = "";
            _messagePopUpUIController.confirmationButtonText.text = "";
            _messagePopUpUIController.declineButtonText.text = "";
            _messagePopUpUIController.onOkayButtonClicked = null;
            _messagePopUpUIController.onCancelButtonClicked = null;
            _messagePopUpUIController.Hide();
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        private void OnOkayClicked()
        {
            ExitThroughNodePort("okay");
        }

        private void OnCancelClicked()
        {
            ExitThroughNodePort("cancel");
        }
    }
}
