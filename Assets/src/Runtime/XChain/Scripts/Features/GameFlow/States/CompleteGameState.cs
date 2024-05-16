using Assets.Scripts.Core.DataProviders.GameDataProvider;
using Core.API;
using Core.App;
using Core.DataProviders.SessionDataProvider;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.GameFlow.States
{
    public class CompleteGameState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        private bool _status;
        public bool status => _status;
        

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync()
        {
            var response = await XChain.Instance.APIService.CompleteGame(XChain.Instance.GetContext().GameContext.SessionID, XChain.Instance.GetContext().SessionContext.AccessToken);
            if (response.IsSuccess)
            {
                _status = response.SuccessResponse.status;
                if (_status)
                {
                    ExitThroughNodePort("success");
                }
                else
                {
                   ExitThroughNodePort("failed"); 
                }
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
