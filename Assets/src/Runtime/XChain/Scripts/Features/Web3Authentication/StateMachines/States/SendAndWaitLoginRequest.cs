using Core.API;
using Core.API.APIParams;
using Core.DataProviders.SessionDataProvider;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using Features.Communication.Types;
using Features.Web3Authentication.StateMachines.Interfaces;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.Web3Authentication.StateMachines.States
{
    public class SendAndWaitLoginRequest: State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync()
        {
            var loginParams = new LoginRequestParams()
            {
                idToken = XChain.Instance.GetContext().Web3Context.IDToken,
                appPubKey = XChain.Instance.GetContext().Web3Context.AppPublicKey
            };

            var response = await XChain.Instance.APIService.Login(loginParams);
            if (response.IsSuccess)
            {
                XChain.Instance.Context.Web3Context.WalletAddress = response.SuccessResponse.user.walletAddress;
                XChain.Instance.Context.SessionContext.AccessToken = response.SuccessResponse.accessToken;
                ExitThroughNodePort("success");
            }
            else
            {
                Debug.Log($"Login Failed: {response.FailureResponse.message}");
                ExitThroughNodePort("failed");
            }
        }

        protected override void Exit()
        {
            
        }

        protected override void UpdateState()
        {
            
        }
    }
}