using System;

namespace Core.API.APIResponse
{
    [Serializable]
    public class FailedResponse
    {
        public string message;
        public string statusCode;
    }
}