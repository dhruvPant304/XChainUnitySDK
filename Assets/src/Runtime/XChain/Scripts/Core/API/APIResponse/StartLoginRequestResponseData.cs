using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.API.APIResponse
{
    [Serializable]
    public class StartLoginRequestResponseData
    {
        public string sessionId;
        public string pageUrl;
    }
}