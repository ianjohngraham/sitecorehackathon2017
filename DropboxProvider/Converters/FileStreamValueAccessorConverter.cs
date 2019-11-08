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
    [SupportedIds("{2C32A982-3960-4F56-BC3B-D4F569CBC548}")]
    public class FileStreamValueAccessorConverter : ValueAccessorConverter
    {
        public FileStreamValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override IValueReader GetValueReader(ItemModel source)
        {
            IValueReader valueReader = base.GetValueReader(source);
            return valueReader ?? new FileStreamValueReader();
        }
    }
}
