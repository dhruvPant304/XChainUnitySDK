using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.API.APIResponse
{
    [Serializable]
    public class StartLoginRequestResponse
    {
        public string sessionId;
        public string url;
    }
}
