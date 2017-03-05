using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
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
            return new CanReadResult()
            {
                CanReadValue = !string.IsNullOrWhiteSpace(((DropBoxFile)source).MetaData.Name)
            };
        }

        public ReadResult Read(object source, DataAccessContext context)
        {
            var metaFile = (DropBoxFile)source;
            var value = metaFile.MetaData.GetType().GetProperty(Property).GetValue(metaFile.MetaData, null);
            return new ReadResult(DateTime.UtcNow) { ReadValue = value, WasValueRead = true };
        }
    }
}
