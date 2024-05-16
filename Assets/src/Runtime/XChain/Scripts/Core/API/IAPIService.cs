using System.Collections.Generic;
using Core.API.APIParams;
using Core.API.APIResponse;
using Cysharp.Threading.Tasks;

namespace Core.API
{
    public interface IAPIService
    {
        public UniTask<RequestResponse<LoginSuccessResponse,FailedResponse>> Login(LoginRequestParams loginParams);
        public UniTask<RequestResponse<StartGameRequestResponse, FailedResponse>> StartGame(string gameID, string token);
        public UniTask<RequestResponse<CompleteGameRequestResponse, FailedResponse>> CompleteGame(string sessionID, string token);
        public UniTask<RequestResponse<GetScoreRequestResponse, FailedResponse>> GetScore(string sessionID, string token);
        public UniTask<RequestResponse<UpdateScoreRequestParams, FailedResponse>> UpdateScore(string sessionID, UpdateScoreRequestParams updateScoreRequestParams, string token);
        public UniTask<RequestResponse<StartLoginRequestResponse, FailedResponse>> StartLogin();
        public UniTask<RequestResponse<CheckLoginRequestResponse, FailedResponse>> CheckLogin(string sessionId);
        public UniTask<RequestResponse<UserDetailsSucessResponse, FailedResponse>> GetUserDetails(string token);
        public UniTask<RequestResponse<OwnedNFTSuccessResponse[], FailedResponse>> GetOwnedNFTDetails(string authToken);
        public UniTask<RequestResponse<ExchangeNetwork[], FailedResponse>> GetExchangeNetworks();
        public UniTask<RequestResponse<Dictionary<string, ExchangePriceResponse>, FailedResponse>> GetPrice(int chainId);
        public UniTask<RequestResponse<float, FailedResponse>> GetXTokenPriceInUSDT(int chainId);
        public UniTask<RequestResponse<ExchangeRateResponse, FailedResponse>> GetExchangeRate(int chainId, string tokenUUID);
        public UniTask<RequestResponse<BalanceResponse, FailedResponse>> GetBalance(int chainId, string tokenUUID, string token);
        public UniTask<RequestResponse<BuyXTokenResponse, FailedResponse>> BuyToken(BuyRequestParams buyParams, string authToken, string accessToken);
    }
}
