using System;
using System.Linq;
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
        public Currency[] currencies;

        public Currency GetCurrency(string tokenName){
            try{
                var currency = currencies
                    .ToList()
                    .Find((c) => c.name == tokenName);
                return currency;
            } catch(Exception e){
                throw new Exception($"Failed to find a curreny with name {tokenName} in network {name}: {e}");
            }
        }
    }
}

