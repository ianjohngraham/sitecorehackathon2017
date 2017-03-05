using Sitecore.DataExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
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
