using System;

namespace Core.API.APIParams
{
    [Serializable]
    public class BuyWithUSDTParams
    {
        public double xTokenAmount;
        public string exchangeRateInUSDT;
        public int chainId;
        public string currencyId;
    }
}
