namespace Core.DataProviders.SessionDataProvider
{
    public interface ISessionDataProvider
    {
        public string AccessToken { get; set; }
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);
    }
}