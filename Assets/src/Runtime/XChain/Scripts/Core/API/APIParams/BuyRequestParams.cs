using System;

namespace Core.API.APIParams
{
    [Serializable]
    public class BuyRequestParams
    {
        public double xTokenAmount;
        public string exchangeRate;
        public int chainId;
        public string currencyId;
    }
}
