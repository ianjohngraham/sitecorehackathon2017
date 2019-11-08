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
    [SupportedIds("{918A7A62-7ABB-4904-AFD7-34A60F899E5E}")]
    public class UniqueNameAccessorConverter : ValueAccessorConverter
    {
        public UniqueNameAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override IValueReader GetValueReader(ItemModel source)
        {
            IValueReader valueReader = base.GetValueReader(source);
            return valueReader ?? new FilenameValueReader();
        }

    }
}
