using System;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class FileStreamValueReader : IValueReader
    {
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
           var dropboxRepository = new DropBoxRepository();
           var response =dropboxRepository.GetFileStreamContent((DropBoxFile) source);

           return new ReadResult(DateTime.UtcNow) { ReadValue = response, WasValueRead = true };
        }      
    }
}
