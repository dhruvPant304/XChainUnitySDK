using Core.DataProviders.SessionDataProvider;

namespace Assets.Scripts.Core.DataProviders.GameDataProvider
{
    public class GameSessionDataProvider : IGameSessionDataProvider
    {
        public int Score { get; set; }
        public string SessionID { get; set; }
    }
}