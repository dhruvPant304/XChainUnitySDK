namespace Assets.Scripts.Core.DataProviders.GameDataProvider
{
    public interface IGameSessionDataProvider
    {
        public int Score { get; set; }
        public string SessionID { get; set; }
    }
}