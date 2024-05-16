using System;

namespace Core.API.APIResponse
{
    [Serializable]
    public class CheckLoginRequestResponse
    {
        public bool status;
        public string idToken;
        public string appPubKey;
        public string privateKey;
    }
}