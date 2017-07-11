using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };


            var config = new DropboxClientConfig(file.Settings.ApplicationName)
            {
                HttpClient = httpClient
            };

            var client = new DropboxClient(file.Settings.AccessToken, config);

            var response = await client.Files.UploadAsync(file.Settings.RootPath + "/" + file.FileName, WriteMode.Overwrite.Instance, body: file.FileStream);

            return true;
        }

        public Metadata GetMetadata(DropBoxFile file)
        {
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };


            var config = new DropboxClientConfig(file.Settings.ApplicationName)
            {
                HttpClient = httpClient
            };

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

