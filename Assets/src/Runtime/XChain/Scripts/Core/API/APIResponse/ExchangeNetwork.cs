using System;
using Types;
using UnityEngine;

namespace Core.API.APIResponse
{
    [Serializable]
    public class ExchangeNetwork
    {
        public string id;
        public string name;
        public string defaultProvider;
        public string wsURL;
        public string rpcURL;
        public int chainId;
        public string explorerURL;
        public string status;
        public Currencies[] currencies;
    }
}

