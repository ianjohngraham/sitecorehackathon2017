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
    public class FilenameValueReader : IValueReader
    {

     
        public FilenameValueReader()
        {
   
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
            var nameValue = ((DropBoxFile)source).MetaData.Name;

            string stringVal = System.IO.Path.GetFileNameWithoutExtension(nameValue);
            return new ReadResult(DateTime.UtcNow) { ReadValue = stringVal, WasValueRead = true };
        }
    }
}
