using System;

namespace Core.API.APIResponse
{
    [Serializable]
    public class CheckLoginRequestResponseData
    {
        public string status;
        public string idToken;
        public string appPubKey;
    }
}
