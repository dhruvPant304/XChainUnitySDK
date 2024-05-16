using System;
using Core.UI.Interfaces;
using UnityEngine;

namespace Core.UI
{
    public abstract class View: MonoBehaviour, IView
    {
        [SerializeField] bool disableOnAwake = true;

        private void Awake()
        {
            if(disableOnAwake) Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        public abstract void OnShow();
        public abstract void OnHide();
    }
}