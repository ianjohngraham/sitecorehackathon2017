using DropboxProvider.Models;
using Sitecore.DataExchange.Models;

namespace DropboxProvider.Helpers
{
        public static class EndpointExtensions
        {
            public static DropboxSettings GetDropboxSettings(this Endpoint endpoint)
            {
                return endpoint.GetPlugin<DropboxSettings>();
            }
            public static bool HasDropboxSettings(this Endpoint endpoint)
            {
                return (GetDropboxSettings(endpoint) != null);
            }
        }

}
