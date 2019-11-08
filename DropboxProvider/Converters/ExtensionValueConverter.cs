using DropboxProvider.Readers;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    [SupportedIds( "{1A53910C-B245-457A-AB02-FE033E9E47AA}")]
    public class ExtensionValueConverter : BaseItemModelConverter<IValueReader>
    {
        ////TODO: REname it to ExtensionValueReaderConverter
        public ExtensionValueConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override ConvertResult<IValueReader> ConvertSupportedItem(ItemModel source)
        {
            return PositiveResult(new ExtensionValueReader());
        }
    }
}
