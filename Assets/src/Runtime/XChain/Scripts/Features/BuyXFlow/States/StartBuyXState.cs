using Cysharp.Threading.Tasks;
using Features.BuyXFlow.UIControllers;
using Features.Communication.Singletons;
using Features.Web3Authentication.UIControllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using XNodeStateMachine;

namespace Features.BuyXFlow.States
{
    public class StartBuyXState : State
    {
        [Output] public NodePort success;
        [Output] public NodePort failed;

        protected override void Enter()
        {
            SendRequestAsync();
        }

        private async UniTask SendRequestAsync() {
            Debug.Log("sending request ");
            var response = await XChain.Instance.APIService.GetExchangeNetworks();
            if (response != null && response.IsSuccess) {
                Debug.Log(response.SuccessResponse.ToList());
                XChain.Instance.Context.BuyXContext.exchangeNetworks = response.SuccessResponse.ToList();
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
