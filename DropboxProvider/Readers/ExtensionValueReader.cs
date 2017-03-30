using Dropbox.Api;
using Dropbox.Api.Files;
using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
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
