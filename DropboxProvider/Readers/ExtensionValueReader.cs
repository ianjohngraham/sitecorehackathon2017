using System;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class ExtensionValueReader : IValueReader
    {

        public CanReadResult CanRead(object source, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return new CanReadResult()
            {
                CanReadValue = !string.IsNullOrWhiteSpace(((string)source))
            };
        }

        public ReadResult Read(object source, DataAccessContext context)
        {
            string stringVal = System.IO.Path.GetExtension((string)source).Replace(".", "");
            return new ReadResult(DateTime.UtcNow) { ReadValue = stringVal, WasValueRead = true };
        }
    }
}
