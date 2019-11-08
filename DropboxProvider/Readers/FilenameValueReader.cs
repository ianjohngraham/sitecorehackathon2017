using System;
using DropboxProvider.Models;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class FilenameValueReader : IValueReader
    {
        public FilenameValueReader()
        {
   
        }

        public ReadResult Read(object source, DataAccessContext context)
        {
            if (string.IsNullOrWhiteSpace(((DropBoxFile)source).MetaData.Name))
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }

            var nameValue = ((DropBoxFile)source).MetaData.Name;

            string stringVal = System.IO.Path.GetFileNameWithoutExtension(nameValue);
            return new ReadResult(DateTime.UtcNow) { ReadValue = stringVal, WasValueRead = true };
        }
    }
}
