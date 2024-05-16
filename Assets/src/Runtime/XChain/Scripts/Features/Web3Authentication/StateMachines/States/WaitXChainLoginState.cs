using Assets.Scripts.Features.Communication.Enums;
using Core.API;
using Core.API.APIParams;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using Features.Communication.Types;
using Features.Web3Authentication.StateMachines.Interfaces;
using Features.Web3Authentication.UIControllers;
using UnityEngine;
using XNode;
using XNodeStateMachine;
using static XNode.Node;

namespace Features.Web3Authentication.StateMachines.States
{
    public class WaitXChainLoginState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        private IWeb3AuthFlowBlackBoard _web3AuthFlowBlackBoard;
        private WaitAuthenticationUIController _waitAuthenticationUIController;
        private NavigationUIController _navigationUIController;
        
        public void Inject(IWeb3AuthFlowBlackBoard web3AuthFlowBlackBoard,
            WaitAuthenticationUIController waitAuthenticationUIController,
            NavigationUIController navigationUIController)
        {
            _web3AuthFlowBlackBoard = web3AuthFlowBlackBoard;
            _waitAuthenticationUIController = waitAuthenticationUIController;
            _navigationUIController = navigationUIController;
        }

        protected override void Enter()
        {
            XChain.StartLoginFlow();
            XChain.SubscribeToEvent(XChainEvents.LoginSuccess, OnLoginSuccess);
            XChain.SubscribeToEvent(XChainEvents.LoginFailed, OnLoginFailure);
            XChain.SubscribeToEvent(XChainEvents.LoginCancelled, OnLoginFailure);
            _waitAuthenticationUIController.Show();
            _navigationUIController.onBackButtonClicked += OnBackButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            XChain.CancelOperation();
        }

        private void OnLoginSuccess(XChainContext context)
        {
            ExitThroughNodePort("success");
        }

        private void OnLoginFailure(XChainContext context)
        {
            ExitThroughNodePort("failed");
        }

        protected override void Exit()
        {
            XChain.UnSubscribeToXChainEvent(XChainEvents.LoginSuccess, OnLoginSuccess);
            XChain.UnSubscribeToXChainEvent(XChainEvents.LoginFailed, OnLoginFailure);
            XChain.UnSubscribeToXChainEvent(XChainEvents.LoginCancelled, OnLoginFailure);
            _navigationUIController.onBackButtonClicked -= OnBackButtonClicked;
            _waitAuthenticationUIController.Hide();
        }

        protected override void UpdateState()
        {
        }
    }
}
