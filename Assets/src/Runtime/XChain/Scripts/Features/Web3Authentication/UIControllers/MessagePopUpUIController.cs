using Core.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Web3Authentication.UIControllers
{
    public class MessagePopUpUIController : View
    {
        [SerializeField] private Button okayButton;
        [SerializeField] private Button cancelButton;

        public TextMeshProUGUI confirmationButtonText;
        public TextMeshProUGUI declineButtonText;

        public TextMeshProUGUI bodyText;
        public TextMeshProUGUI headerText;

        public Action onOkayButtonClicked;
        public Action onCancelButtonClicked;

        private void OnOkayButtonClicked()
        {
            onOkayButtonClicked?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            onCancelButtonClicked?.Invoke();
        }

        public override void OnShow()
        {
            okayButton.onClick.AddListener(OnOkayButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        public override void OnHide()
        {
            okayButton.onClick.RemoveListener(OnOkayButtonClicked);
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }
    }
}
