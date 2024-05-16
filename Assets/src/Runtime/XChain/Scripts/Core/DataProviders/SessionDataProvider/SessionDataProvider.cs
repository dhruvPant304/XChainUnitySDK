namespace Core.DataProviders.SessionDataProvider
{
    public class SessionDataProvider : ISessionDataProvider
    {
        public string AccessToken { get; set; }
        public string SessionID { get; set; }
        public int Score { get; set; }
    }
}