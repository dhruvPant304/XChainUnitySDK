using System;

namespace Core.API.APIResponse
{
    [Serializable]

    public class BuyWithUSDTResponse
    {
        public string nonce;
        public string from;
        public string to;
        public string data;
        public int gas;
    }
}
