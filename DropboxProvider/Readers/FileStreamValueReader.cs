using System;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class FileStreamValueReader : IValueReader
    {

        public ReadResult Read(object source, DataAccessContext context)
        {
            if (string.IsNullOrWhiteSpace(((DropBoxFile) source).MetaData.Name))
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }

            var dropboxRepository = new DropBoxRepository();
            var response =dropboxRepository.GetFileStreamContent((DropBoxFile) source);

            return new ReadResult(DateTime.UtcNow) { ReadValue = response, WasValueRead = true };
        }      
    }
}
