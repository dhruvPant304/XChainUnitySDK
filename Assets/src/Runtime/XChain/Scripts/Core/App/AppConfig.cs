using System;
using UnityEngine;

namespace Core.App
{
    [Serializable]
    public class AppConfig : SavedConfigData<AppConfig>
    {
        public NetworkSettings networkSettings = new NetworkSettings();
        public GameSettings gameSettings = new GameSettings();
    }
}