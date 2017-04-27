using System;
using DropboxProvider.Readers;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    public class MetaDataValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{6CCAB5E5-5EA3-4B19-9144-29C97C0572AD}");
        public MetaDataValueAccessorConverter(IItemModelRepository repository) : base(repository)
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
             var property = base.GetStringValue(source, "Property");
           
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new MetaDataPropertyValueReader(property);
            }

            return accessor;
        }

    }
}
