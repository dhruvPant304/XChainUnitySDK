using Assets.Scripts.Core.DataProviders.GameDataProvider;
using Core.API;
using Core.API.APIParams;
using Core.DataProviders.SessionDataProvider;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.GameFlow.States
{
    public class UpdateScoreState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync()
        {
            var updateScoreRequestParams = new UpdateScoreRequestParams()
            {
                score = XChain.Instance.GetContext().GameContext.Score
            };
            var response = await XChain.Instance.APIService.UpdateScore(XChain.Instance.GetContext().GameContext.SessionID, updateScoreRequestParams, XChain.Instance.GetContext().SessionContext.AccessToken);
            if (response.IsSuccess)
            {
                XChain.Instance.Context.GameContext.Score = response.SuccessResponse.score;
                ExitThroughNodePort("success");
            }
            else
            {
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
