using System.Linq;
using Features.BuyXFlow.UIControllers;
using Features.Communication.Singletons;
using UnityEngine;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States{
    public class FetchExchangeNetworks : State
    {
        [Output] public NodePort exit;

        protected override async void Enter() {
            Debug.Log("sending request ");
            var response = await XChain.Instance.APIService.GetExchangeNetworks();
            if (response != null && response.IsSuccess) {
                Debug.Log(response.SuccessResponse.ToList());
                XChain.Instance.Context.BuyXContext.exchangeNetworks = response.SuccessResponse.ToList();
                XChainCanvas.Instance.OpenBuyXView();
                ExitThroughNodePort("exit");
            } 
        }

        protected override void Exit() {
        }

        protected override void UpdateState() {
        }
    }
}

