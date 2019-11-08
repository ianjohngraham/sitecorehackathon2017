using Sitecore.DataExchange;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Models
{
    public class ResolveDropboxFileSettings : IPlugin
    {

        public IValueAccessor MatchingFieldValueAccessor { get; set; }

        public IValueAccessor ItemNameValueAccessor { get; set; }

    }
}
