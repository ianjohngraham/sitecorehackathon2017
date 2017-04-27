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

        public CanReadResult CanRead(object source, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var nameValue = ((DropBoxFile)source).MetaData.Name;

            nameValue = System.IO.Path.GetFileNameWithoutExtension(nameValue);

            return new CanReadResult()
            {
                CanReadValue = !string.IsNullOrWhiteSpace(nameValue)
            };
        }

        public ReadResult Read(object source, DataAccessContext context)
        {
            var metaFile = (DropBoxFile)source;
            var value = metaFile.MetaData.GetType().GetProperty(Property).GetValue(metaFile.MetaData, null);
            var stringVal = (string)value;

            return new ReadResult(DateTime.UtcNow) { ReadValue = stringVal, WasValueRead = true };
        }
    }
}
