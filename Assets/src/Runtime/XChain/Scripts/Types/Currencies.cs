using System;

namespace Types
{
    [Serializable]
    public class Currency
    {
        public string id;
        public string name;
        public string networkId;
        public string tokenAddress;
        public string symbol;
        public string status;
        public string logoURL;
        public bool isNativeToken; 
    }
}
