using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using DropboxProvider.Models;
using Sitecore.Diagnostics;

namespace DropboxProvider.Repository
{
    public class DropBoxRepository
    {
        public async Task<bool> Update(DropBoxFile file)
        {
            var config = GetConfig(file.Settings);
            var client = new DropboxClient(file.Settings.AccessToken, config);

            var response = await client.Files.UploadAsync(file.Settings.RootPath + "/" + file.FileName, WriteMode.Overwrite.Instance, body: file.FileStream);

            return true;
        }

        private DropboxClientConfig GetConfig(DropboxSettings settings)
        {
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            var config = new DropboxClientConfig(settings.ApplicationName)
            {
                HttpClient = httpClient
            };

            return config;
        }


        public string GetFileStreamContent(DropBoxFile source)
        {
            var config = GetConfig(source.Settings);
            var client = new DropboxClient(source.Settings.AccessToken, config);

            FileMetadata fileData = (FileMetadata)source.MetaData;
            var response = Download(client, fileData).Result;

            return response;
        }

        private async Task<string> Download(DropboxClient client, FileMetadata file)
        {
            using (var response = await client.Files.DownloadAsync(file.PathLower))
            {
                var bytes = await response.GetContentAsByteArrayAsync();
                return Convert.ToBase64String(bytes);
            }
        }

        public IEnumerable<DropBoxFile> ReadAll(DropboxSettings settings)
        {
            var config = GetConfig(settings);
            var client = new DropboxClient(settings.AccessToken, config);

            var entries = client.Files.ListFolderAsync(settings.RootPath).Result.Entries.Where(e => e.IsFile).ToList();
            var dropboxFiles = entries.Select(entry => new DropBoxFile(entry, settings));
            return dropboxFiles;
        }

        public Metadata GetMetadata(DropBoxFile file)
        {
            var config = GetConfig(file.Settings);
            var client = new DropboxClient(file.Settings.AccessToken, config);
            try
            {
                var response = client.Files.AlphaGetMetadataAsync(file.Settings.RootPath + "/" + file.FileName).Result;
                return response;
            }
            catch (Exception ex)
            {
                Log.Info("Can't find file",ex);
            } 
            return null;
        }
    }
}

