using Cysharp.Threading.Tasks;
using Features.BuyXFlow.UIControllers;
using Features.Communication.Singletons;
using System.Linq;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States
{
    public class StartBuyXState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        protected override async void Enter()
        {
            await SendRequestAsync();
        }

        private async UniTask SendRequestAsync() {
            Debug.Log("sending request ");
            var response = await XChain.Instance.APIService.GetExchangeNetworks();
            if (response != null && response.IsSuccess) {
                Debug.Log(response.SuccessResponse.ToList());
                XChain.Instance.Context.BuyXContext.exchangeNetworks = response.SuccessResponse.ToList();
                XChainCanvas.Instance.OpenBuyXView();
                ExitThroughNodePort("success");
            }
            else {
                Debug.Log($"Fetch Exchange Networks Failed: {response.FailureResponse}");
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
