using System;
using DropboxProvider.Readers;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    public class UniqueNameAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{918A7A62-7ABB-4904-AFD7-34A60F899E5E}");
        public UniqueNameAccessorConverter(IItemModelRepository repository) : base(repository)
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
             var property = base.GetStringValue(source, "Name");
           
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new FilenameValueReader();
            }

            return accessor;
        }

    }
}
