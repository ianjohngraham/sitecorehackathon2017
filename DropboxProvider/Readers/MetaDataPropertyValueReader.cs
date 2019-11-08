using System;
using DropboxProvider.Models;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class MetaDataPropertyValueReader : IValueReader
    {
        public string Property { get; set; }

        public MetaDataPropertyValueReader(string property)
        {
            Property = property;
        }

        public string FieldName { get; private set; }

        public ReadResult Read(object source, DataAccessContext context)
        {
            var nameValue = ((DropBoxFile)source).MetaData.Name;

            nameValue = System.IO.Path.GetFileNameWithoutExtension(nameValue);

            if (string.IsNullOrWhiteSpace(nameValue))
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }

            var metaFile = (DropBoxFile)source;
            var value = metaFile.MetaData.GetType().GetProperty(Property).GetValue(metaFile.MetaData, null);
            var stringVal = (string)value;

            return new ReadResult(DateTime.UtcNow) { ReadValue = stringVal, WasValueRead = true };
        }
    }
}
