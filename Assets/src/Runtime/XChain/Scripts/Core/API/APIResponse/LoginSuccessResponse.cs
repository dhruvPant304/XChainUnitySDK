using System;
using Types;

namespace Core.API.APIResponse
{
    [Serializable]
    public class LoginSuccessResponse
    {
        public string accessToken;
        public UserDetails user;
        public string privateKey;
    }
}