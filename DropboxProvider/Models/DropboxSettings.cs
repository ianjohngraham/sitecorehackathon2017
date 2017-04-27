namespace DropboxProvider.Models
{

    public class DropboxSettings : Sitecore.DataExchange.IPlugin
    {
        public DropboxSettings()
        {
        }
        public string ApplicationName { get; set; }
        public string AccessToken { get; set; }

        public string RootPath { get; set; }
    }
}
