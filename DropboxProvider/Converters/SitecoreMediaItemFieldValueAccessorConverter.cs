using DropboxProvider.FieldWriter;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.Sc.DataAccess.Readers;
using Sitecore.DataExchange.Providers.Sc.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider.Converters
{

    [SupportedIds(new string[] { "{6F7375CC-9942-4E30-8C63-95281B69C65F}" })]
    public class SitecoreMediaItemFieldValueAccessorConverter : ValueAccessorConverter
    {
        public SitecoreMediaItemFieldValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }

        public override IValueAccessor Convert(ItemModel source)
        {
            IValueAccessor valueReader = base.Convert(source);
            if (valueReader == null)
            {
                return null;
            }
            ItemModel referenceAsModel = base.GetReferenceAsModel(source, "Field");
            if (referenceAsModel == null)
            {
                return null;
            }
            string item = referenceAsModel["ItemName"] as string;
            if (string.IsNullOrWhiteSpace(item))
            {
                return null;
            }
            string language = this.GetLanguage(source);
            if (valueReader.ValueReader == null)
            {
                valueReader.ValueReader = this.GetValueReader(item, source, language);
            }
            if (valueReader.ValueWriter == null)
            {
                valueReader.ValueWriter = this.GetValueWriter(item, source, language);
            }
            return valueReader;
        }

        protected virtual string GetLanguage(ItemModel source)
        {
            return base.GetStringValue(source, "Language");
        }

        protected virtual IValueReader GetValueReader(string fieldName, ItemModel source, string language = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                return new SitecoreItemFieldReader(fieldName);
            }
            return new ItemModelFieldFromStringDictionaryReader(language, fieldName);
        }

        protected virtual IValueWriter GetValueWriter(string fieldName, ItemModel source, string language = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                return new SitecoreMediaItemFieldWriter(fieldName);
            }
            return new ItemModelFieldFromStringDictionaryWriter(language, fieldName);
        }
    }
}
