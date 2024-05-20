using System.Collections.Generic;
using System.Text;
using Core.API.APIParams;
using Core.API.APIResponse;
using Core.App;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Types;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.API
{
    public class APIService : IAPIService
    {
        private readonly NetworkSettings _networkSettings;

        public APIService(NetworkSettings networkSettings)
        {
            _networkSettings = networkSettings;
        }
        private string ServerUrl => _networkSettings.serverUrl;
        private string XChainAuthUrl => _networkSettings.xChainAuthUrl;

        public async UniTask<RequestResponse<LoginSuccessResponse, FailedResponse>> Login(LoginRequestParams loginParams)
        {
            var url = ServerUrl + "/api/v1/auth/registerWeb3Auth";
            return await SendWebRequest<LoginSuccessResponse, FailedResponse>(url, "POST", loginParams);
        }

        public async UniTask<RequestResponse<StartGameRequestResponse, FailedResponse>> StartGame(string gameID, string token)
        {
            var url = ServerUrl + $"/api/v1/{gameID}/start";
            return await SendWebRequest<StartGameRequestResponse, FailedResponse>(url, "POST", "", token);
        }

        public async UniTask<RequestResponse<CompleteGameRequestResponse, FailedResponse>> CompleteGame(string sessionID, string token)
        {
            var url = ServerUrl + $"/api/v1/{sessionID}/complete";
            return await SendWebRequest<CompleteGameRequestResponse, FailedResponse>(url, "PATCH", "", token);
        }

        public async UniTask<RequestResponse<GetScoreRequestResponse, FailedResponse>> GetScore(string sessionID, string token)
        {
            var url = ServerUrl + $"/api/v1/{sessionID}/score";
            return await SendWebRequest<GetScoreRequestResponse, FailedResponse>(url, "GET", token);
        }

        public async UniTask<RequestResponse<UpdateScoreRequestParams, FailedResponse>> UpdateScore(string sessionID, UpdateScoreRequestParams updateScoreRequestParams, string token)
        {
            var url = ServerUrl + $"/api/v1/{sessionID}/score";
            return await SendWebRequest<UpdateScoreRequestParams, FailedResponse>(url, "POST", updateScoreRequestParams, token);
        }

        public async UniTask<RequestResponse<StartLoginRequestResponse, FailedResponse>> StartLogin()
        {
            var url = XChainAuthUrl + $"/api/v1/login/start";
            return await SendWebRequest<StartLoginRequestResponse, FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<CheckLoginRequestResponse, FailedResponse>> CheckLogin(string sessionId)
        {
            var url = XChainAuthUrl + $"/api/v1/login/check/?sessionId={sessionId}";
            return await SendWebRequest<CheckLoginRequestResponse, FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<UserDetailsSucessResponse, FailedResponse>> GetUserDetails(string token) {
            var url = ServerUrl + $"/api/game-api/v1/auth/whoami";
            return await SendWebRequest<UserDetailsSucessResponse, FailedResponse>(url, "GET", null, token, token);
        }

        public async UniTask<RequestResponse<GetNFTSuccessResponse[], FailedResponse>> GetOwnedNFTDetails(string gameId, string authToken) {
            var url = ServerUrl + $"/api/game-api/v1/nft/ownedNfts";
            return await SendWebRequest<GetNFTSuccessResponse[], FailedResponse>(url, "GET", null, authToken);
        }

        public async UniTask<RequestResponse<GetNFTSuccessResponse[], FailedResponse>> GetAllNFTDetails(string gameId, string collectionId, int perPageAmount, int page) {
            var url = ServerUrl + $"/api/game-api/v1/nft?perPage={perPageAmount}&page={page}";
            return await SendWebRequest<GetNFTSuccessResponse[], FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<BuyNFTRequestResponse, FailedResponse>> BuyNFT(BuyNFTRequestParams buyParams,string nftId, string token) {
            var url = ServerUrl + $"/api/game-api/v1/nft/{nftId}/buy";
            return await SendWebRequest<BuyNFTRequestResponse, FailedResponse>(url, "POST", buyParams, token);
        }

        public async UniTask<RequestResponse<ExchangeNetwork[], FailedResponse>> GetExchangeNetworks()
        {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/networks";
            return await SendWebRequest<ExchangeNetwork[], FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<Dictionary<string, ExchangePriceResponse>, FailedResponse>> GetPrice(int chainId)
        {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/price?chainId={chainId}";
            return await SendWebRequest<Dictionary<string, ExchangePriceResponse>, FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<float, FailedResponse>> GetXTokenPriceInUSDT(int chainId) {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/xtoken-price?chainId={chainId}";
            return await SendWebRequest<float, FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<ExchangeRateResponse, FailedResponse>> GetExchangeRate(int chainId, string tokenUUID) {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/token/price?chainId={chainId}&currencyId={tokenUUID}";
            return await SendWebRequest<ExchangeRateResponse, FailedResponse>(url, "GET");
        }

        public async UniTask<RequestResponse<BalanceResponse, FailedResponse>> GetBalance(int chainId, string tokenUUID, string token)
        {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/balanceOf?chainId={chainId}&tokenUUID={tokenUUID}";
            return await SendWebRequest<BalanceResponse, FailedResponse>(url,"GET" ,null, token, token);
        }

        public async UniTask<RequestResponse<BuyXTokenResponse, FailedResponse>> BuyToken(BuyRequestParams buyParams, string authToken, string accessToken)
        {
            var url = ServerUrl + $"/api/game-api/v1/x-token-exchange/buy";
            return await SendWebRequest<BuyXTokenResponse, FailedResponse>(url, "POST", buyParams, authToken, accessToken);
        }

        private async UniTask<RequestResponse<TSuccess, TFailure>> SendWebRequest<TSuccess,TFailure>(string url, string method, object param = null, string authToken = "", string accessToken = "")
        {
            var request = CreateRequest(url, method, param, authToken, accessToken);
            if(_networkSettings.debugSettings.logResponses)
                Debug.Log($"Sent: {url}");
            await request.SendWebRequest();
            var responseText = request.downloadHandler.text;
            if(_networkSettings.debugSettings.logResponses) 
                Debug.Log($"Received: {responseText}");
            var response = new RequestResponse<TSuccess, TFailure>();
            if (request.result == UnityWebRequest.Result.Success)
            {
                response.IsSuccess = true;
                response.SuccessResponse = JsonConvert.DeserializeObject<TSuccess>(responseText);
            }
            else
            {
                response.IsSuccess = false;
                response.FailureResponse = JsonConvert.DeserializeObject<TFailure>(responseText);
            }
            return response;
        }

        private UnityWebRequest CreateRequest(string uri, string method, object body = null, string authToken = "", string accessToken = "")
        {
            var request = new UnityWebRequest(uri);
            request.method = method;
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeUploadHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;
            if (!string.IsNullOrEmpty(authToken)) {
                request.SetRequestHeader("Authorization", $"Bearer {authToken}");
            }
            if (!string.IsNullOrEmpty(accessToken)) {
                request.SetRequestHeader("access-token", accessToken);
            }
            var json = JsonUtility.ToJson(body);
            if (_networkSettings.debugSettings.logParams)
                Debug.Log($"Sent: {uri} \n body: {json}");
            var data = Encoding.UTF8.GetBytes(json);
            if (body != null) request.uploadHandler = new UploadHandlerRaw(data);
            return request;
        }
    }
}
