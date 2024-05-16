using Assets.Scripts.Core.DataProviders.GameDataProvider;
using Core.API;
using Core.API.APIParams;
using Core.App;
using Core.DataProviders.SessionDataProvider;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.GameFlow.States
{
    public class StartGameState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;
        

        private string _gameID => XChain.Instance.AppConfig.gameSettings.gameID;

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync()
        {
            var response = await XChain.Instance.APIService.StartGame(_gameID, XChain.Instance.GetContext().SessionContext.AccessToken);
            if (response.IsSuccess)
            {
                XChain.Instance.Context.GameContext.SessionID = response.SuccessResponse.sessionId;
                ExitThroughNodePort("success");
            }
            else
            {
                Debug.Log($"Start Game Failed: {response.FailureResponse}");
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
