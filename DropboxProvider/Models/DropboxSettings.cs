using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
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
