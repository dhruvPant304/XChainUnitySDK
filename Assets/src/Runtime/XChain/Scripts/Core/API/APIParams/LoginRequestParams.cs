using System;

namespace Core.API.APIParams
{
    [Serializable]
    public class LoginRequestParams
    {
        public string idToken;
        public string appPubKey;
    }
}