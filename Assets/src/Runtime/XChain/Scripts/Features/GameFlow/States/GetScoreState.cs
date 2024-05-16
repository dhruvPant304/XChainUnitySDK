using Assets.Scripts.Core.DataProviders.GameDataProvider;
using Core.API;
using Core.DataProviders.SessionDataProvider;
using Cysharp.Threading.Tasks;
using Features.Communication.Enums;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.GameFlow.States
{
    public class GetScoreState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync()
        {
            var response = await XChain.Instance.APIService.GetScore(XChain.Instance.GetContext().GameContext.SessionID, XChain.Instance.GetContext().SessionContext.AccessToken);
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
