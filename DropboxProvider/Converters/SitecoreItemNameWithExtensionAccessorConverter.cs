using System;
using DropboxProvider.Readers;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    [SupportedIds(new string[] { "{6F7375CC-9942-4E30-8C63-95281B69C65F}" })]
    public class SitecoreItemNameWithExtensionAccessorConverter : ValueAccessorConverter
    {

        private static readonly Guid TemplateId = Guid.Parse("{2C32A982-3960-4F56-BC3B-D4F569CBC548}");
        public SitecoreItemNameWithExtensionAccessorConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        public override IValueAccessor Convert(ItemModel source)
        {
            var accessor = base.Convert(source);
            if (accessor == null)
            {
                return null;
            }
            
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new SitecoreItemNameReader(true);
            }

            return accessor;
        }

    }
}
