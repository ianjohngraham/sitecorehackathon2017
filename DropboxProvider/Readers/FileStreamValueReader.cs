using Dropbox.Api;
using Dropbox.Api.Files;
using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
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

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            var dropBoxFile = (DropBoxFile)source;

            var config = new DropboxClientConfig(dropBoxFile.Settings.ApplicationName)
            {
                HttpClient = httpClient
            };


            var client = new DropboxClient(dropBoxFile.Settings.AccessToken, config);

            FileMetadata fileData = (FileMetadata)dropBoxFile.MetaData;

            var response = Download(client,  fileData).Result;

            return new ReadResult(DateTime.UtcNow) { ReadValue = response, WasValueRead = true };
        }

        private async Task<string> Download(DropboxClient client, FileMetadata file)
        {
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            using (var response = await client.Files.DownloadAsync( file.PathLower))
            {
                return await response.GetContentAsStringAsync();
            }           
        }
    }
}
