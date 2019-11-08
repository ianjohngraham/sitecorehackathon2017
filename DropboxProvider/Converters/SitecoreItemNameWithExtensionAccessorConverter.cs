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
    [SupportedIds("{6F7375CC-9942-4E30-8C63-95281B69C65F}", "{2C32A982-3960-4F56-BC3B-D4F569CBC548}")]
    public class SitecoreItemNameWithExtensionAccessorConverter : ValueAccessorConverter
    {
        public SitecoreItemNameWithExtensionAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override IValueReader GetValueReader(ItemModel source)
        {
            IValueReader valueReader = base.GetValueReader(source);
            return valueReader ?? new SitecoreItemNameReader(true);
        }
        
    }
}
