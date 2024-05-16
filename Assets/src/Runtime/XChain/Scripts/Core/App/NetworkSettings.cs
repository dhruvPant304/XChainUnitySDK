using System;
using UnityEngine;

namespace Core.App
{
    [Serializable]
    public class NetworkSettings
    {
        public string serverUrl;
        public string xChainAuthUrl;
        public DebugSettings debugSettings = new DebugSettings();
    }

    [Serializable]
    public class DebugSettings
    {
        public bool logResponses;
        public bool logParams;
    }
}