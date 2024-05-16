using Core.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Web3Authentication.UIControllers
{
    public class NavigationUIController : View
    {
        [SerializeField] private Button backButton;

        public Action onBackButtonClicked;

        private void OnBackButtonClicked()
        {
            onBackButtonClicked?.Invoke();
        }

        public override void OnShow()
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        public override void OnHide()
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }

        public void HideBackButton()
        {
            backButton.gameObject.SetActive(false);
        }

        public void ShowBackButton()
        {
            backButton.gameObject.SetActive(true);
        }
    }
}
