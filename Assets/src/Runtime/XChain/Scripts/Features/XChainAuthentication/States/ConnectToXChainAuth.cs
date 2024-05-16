using System;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;
using Application = UnityEngine.Application;

namespace Features.XChainAuthentication.States
{
    public class ConnectToXChainAuth : State
    {
        private string _sessionId;
        [Output] public NodePort success;
        [Output] public NodePort failed;
        protected override void Enter()
        {
            EstablishConnection();
        }

        protected override void Exit()
        {
            //throw new System.NotImplementedException();
        }

        protected override void UpdateState()
        {
            //throw new System.NotImplementedException();
        }

        private async UniTask EstablishConnection()
        {
            await SendRequest();
            await CheckLoginRoutine();
        }
        private async UniTask SendRequest()
        {
            var response = await XChain.Instance.APIService.StartLogin();
            if (response.IsSuccess)
            {
                var url = response.SuccessResponse.url;
                _sessionId = response.SuccessResponse.sessionId;
                Debug.Log($"SessionId: {_sessionId}");
                Application.OpenURL(url);
            }
        }

        private async UniTask CheckLoginRoutine()
        {
            var response = await XChain.Instance.APIService.CheckLogin(_sessionId);
            if (response.IsSuccess)
            {
                if (response.SuccessResponse.status)
                {
                    Debug.Log($"idToken: {response.SuccessResponse.idToken}");
                    Debug.Log($"public Key: {response.SuccessResponse.appPubKey}");
                    XChain.Instance.Context.SessionContext.AccessToken = response.SuccessResponse.idToken;
                    XChain.Instance.Context.Web3Context.WalletAddress =response.SuccessResponse.appPubKey;
                    ExitThroughNodePort("success");
                }
                else
                {
                    await UniTask.Delay(1000);
                    Debug.Log($"User login status {response.SuccessResponse.status}");
                    await CheckLoginRoutine();
                }
            }
            else
            {
                Debug.LogError("Check Login Failed");
                ExitThroughNodePort("failed");
            }
        }
    }
}