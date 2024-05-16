using System;
using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Web3Authentication.UIControllers
{
    public class LandingPageUIController : View
    {
        [SerializeField] private Button loginButton;
        [SerializeField] private Button signUpButton;

        public Action onLoginButtonClicked;
        public Action onSignUpButtonClicked;

        private void OnLoginButtonClicked()
        {
            onLoginButtonClicked?.Invoke();
        }

        private void OnSignUpButtonClicked()
        {
            onSignUpButtonClicked?.Invoke();
        }

        public override void OnShow()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            signUpButton.onClick.AddListener(OnSignUpButtonClicked);
        }

        public override void OnHide()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClicked);
            signUpButton.onClick.RemoveListener(OnSignUpButtonClicked);
        }
    }
}
