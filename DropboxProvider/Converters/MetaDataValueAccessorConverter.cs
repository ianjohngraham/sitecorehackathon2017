using System;
using DropboxProvider.Readers;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    [SupportedIds("{6CCAB5E5-5EA3-4B19-9144-29C97C0572AD}")]
    public class MetaDataValueAccessorConverter : ValueAccessorConverter
    {
        public MetaDataValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override IValueReader GetValueReader(ItemModel source)
        {
            IValueReader valueReader = base.GetValueReader(source);
            return valueReader ?? new MetaDataPropertyValueReader(GetStringValue(source, "Property"));
        }

    }
}
