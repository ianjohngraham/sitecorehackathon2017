using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DropBoxConnector
{
    public class DropboxExample
    {
        public async void Test()
        {

            var accessToken = "WebSO5Ej1BkAAAAAAAAMf73UzlQ7sSGkEksdFsP_guryEHH7zXZ-q_wLtQbuMAfH";


            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                // Specify request level timeout which decides maximum time taht can be spent on
                // download/upload files.
                Timeout = TimeSpan.FromMinutes(20)
            };


            var config = new DropboxClientConfig("SitecoreConnector")
            {
                HttpClient = httpClient
            };

            var client = new DropboxClient(accessToken, config);

            var list = await client.Files.ListFolderAsync("/Stuff");

            var fileName = @"C:\Development\Webprojects\OneDrivePOC\OneDrive\DropBoxConnector\Media\host.PNG";

            string filestuff = File.ReadAllText(fileName);

            // await Upload(client, "/Stuff", "host.PNG", filestuff);

            var file = list.Entries.FirstOrDefault(i => i.IsFile);

            var result = Download(client, "", (Metadata)file).Result;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={file.Name}");
            HttpContext.Current.Response.AddHeader("Content-Length", result.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/zip";
            HttpContext.Current.Response.Write(result);
            HttpContext.Current.Response.End();
        }

        private async Task<string> Download(DropboxClient client, string folder, Metadata file)
        {
            Console.WriteLine("Download file...");

            using (var response = await client.Files.DownloadAsync(file.PathLower))
            {
                return await response.GetContentAsStringAsync();

            }
        }

        /// <summary>
        /// Uploads given content to a file in Dropbox.
        /// </summary>
        /// <param name="client">The Dropbox client.</param>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileContent">The file content.</param>
        /// <returns></returns>
        private async Task Upload(DropboxClient client, string folder, string fileName, string fileContent)
        {
            Console.WriteLine("Upload file...");

            using (var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(fileContent)))
            {
                var response = await client.Files.UploadAsync(folder + "/" + fileName, WriteMode.Overwrite.Instance, body: stream);

                Console.WriteLine("Uploaded Id {0} Rev {1}", response.Id, response.Rev);
            }
        }

        private async Task ChunkUpload(DropboxClient client, string folder, string fileName)
        {
            Console.WriteLine("Chunk upload file...");
            // Chunk size is 128KB.
            const int chunkSize = 128 * 1024;

            // Create a random file of 1MB in size.
            var fileContent = new byte[1024 * 1024];
            new Random().NextBytes(fileContent);

            using (var stream = new MemoryStream(fileContent))
            {
                int numChunks = (int)Math.Ceiling((double)stream.Length / chunkSize);

                byte[] buffer = new byte[chunkSize];
                string sessionId = null;

                for (var idx = 0; idx < numChunks; idx++)
                {
                    Console.WriteLine("Start uploading chunk {0}", idx);
                    var byteRead = stream.Read(buffer, 0, chunkSize);

                    using (MemoryStream memStream = new MemoryStream(buffer, 0, byteRead))
                    {
                        if (idx == 0)
                        {
                            var result = await client.Files.UploadSessionStartAsync(body: memStream);
                            sessionId = result.SessionId;
                        }

                        else
                        {
                            UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(chunkSize * idx));

                            if (idx == numChunks - 1)
                            {
                                await client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName), memStream);
                            }

                            else
                            {
                                await client.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                            }
                        }
                    }
                }
            }
        }

        //private async Task<string> GetAccessToken()
        //{



        //    if (string.IsNullOrEmpty(accessToken))
        //    {
        //        Console.WriteLine("Waiting for credentials.");
        //        var completion = new TaskCompletionSource<Tuple<string, string>>();

        //        var thread = new Thread(() =>
        //        {
        //            try
        //            {
        //                var app = new Application();
        //                var login = new LoginForm(ApiKey);
        //                app.Run(login);
        //                if (login.Result)
        //                {
        //                    completion.TrySetResult(Tuple.Create(login.AccessToken, login.Uid));
        //                }
        //                else
        //                {
        //                    completion.TrySetCanceled();
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                completion.TrySetException(e);
        //            }
        //        });
        //        thread.SetApartmentState(ApartmentState.STA);
        //        thread.Start();

        //        try
        //        {
        //            var result = await completion.Task;
        //            Console.WriteLine("and back...");

        //            accessToken = result.Item1;
        //            var uid = result.Item2;
        //            Console.WriteLine("Uid: {0}", uid);

        //            Settings.Default.AccessToken = accessToken;
        //            Settings.Default.Uid = uid;

        //            Settings.Default.Save();
        //        }
        //        catch (Exception e)
        //        {
        //            e = e.InnerException ?? e;
        //            Console.WriteLine("Error: {0}", e.Message);
        //            return null;
        //        }
        //    }

        //    return accessToken;
        //}
    }
}