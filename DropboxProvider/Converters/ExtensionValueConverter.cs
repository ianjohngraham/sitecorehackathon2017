using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
{
    [SupportedIds(new string[] { "{1A53910C-B245-457A-AB02-FE033E9E47AA}" })]
    public class ExtensionValueConverter : BaseItemModelConverter<ItemModel, IValueReader>
    {
        public ExtensionValueConverter(IItemModelRepository repository) : base(repository)
        {
        }

        public override IValueReader Convert(ItemModel source)
        {
            if (source == null)
            {
                Context.Logger.Error("Cannot convert null item to value reader. (converter: {0})", new object[] { base.GetType().FullName });
                return null;
            }
            if (!this.CanConvert(source))
            {
                Context.Logger.Error("Cannot convert item to value reader. (item: {0}, converter: {1})", new object[] { source.GetItemId(), base.GetType().FullName });
                return null;
            }
         
            return new ExtensionValueReader();
        }

    }
}
