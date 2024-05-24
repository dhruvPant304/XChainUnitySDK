using Core.UI;
using Cysharp.Threading.Tasks;
using Features.Communication.Singletons;
using UnityEngine;

namespace Features.BuyXFlow.UIControllers
{
    public class BuyNFTController : View
    {
        [SerializeField] string accessToken;
        [SerializeField] string accessKey = "283c238b82d0cfba454f9b01a7c205bd";
        private string _accessToken {
            get {
                #if UNITY_EDITOR
                return accessToken;
                #endif
                return XChain.Instance.Context.SessionContext.AccessToken;
            }
        }

        private string _accessKey {
            get {
                #if UNITY_EDITOR
                return accessKey;
                #endif
                return XChain.Instance.Context.Web3Context.AccessKey;
            }
        }
        
        public async void GetUserNFTDetails() {
            await GetNFTDetails();
        }

        private async UniTask GetNFTDetails() {
            var response = await XChain.Instance.APIService.GetOwnedNFTDetails(XChain.Instance.AppConfig.gameSettings.gameID, _accessToken);
            if (response.IsSuccess) {
                Debug.Log(response.SuccessResponse);
            }
            else {
                Debug.Log($"Get User NFT Details Failed: {response.FailureResponse.message}");
            }
        }

        public async void GetAllNFTDetails() {
            await GetAllNFTsDetails();
        }

        private async UniTask GetAllNFTsDetails() {
            string _collectionId = ""; //add collection id here to filter nft details with collection
            int _perPageAmount = 10;
            int _page = 1;
            var response = await XChain.Instance.APIService.GetAllNFTDetails(XChain.Instance.AppConfig.gameSettings.gameID, _collectionId, _perPageAmount,_page);
            if (response.IsSuccess) {
                Debug.Log(response.SuccessResponse);
            }
            else {
                Debug.Log($"Get All NFT Details Failed: {response.FailureResponse.message}");
            }
        }

        
        // }
        // private async UniTask BuyUserNFT() {
        //     string _nftId = ""; //add nftId here to purchase
        //     var buyParams = new BuyNFTRequestParams() {
        //         quantity = _nftQuantity
        //     };
        //     var response = await XChain.Instance.APIService.BuyNFT(buyParams, _nftId, _accessToken);
        //     if (response.IsSuccess) {
        //         Debug.Log(response.SuccessResponse);
        //     }
        //     else {
        //         Debug.Log($"Purchase Failed: {response.FailureResponse.message}");
        //     }
        // }

        public override void OnHide(){

        }

        public override void OnShow(){

        }
    }
}
