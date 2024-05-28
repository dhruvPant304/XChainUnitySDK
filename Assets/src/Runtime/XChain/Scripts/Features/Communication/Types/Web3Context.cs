using Core.API.APIResponse;

namespace Features.Communication.Types
{
    public struct Web3Context
    {
        public UserDetails UserData;
        public string AccessKey;
        public string IDToken;
        public string AppPublicKey;
    }
}