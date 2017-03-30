using Dropbox.Api;
using Sitecore.Data.Items;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
{
    [RequiredEndpointPlugins(typeof(DropboxSettings))]
    public class ReadDropboxStepProcessor : BaseReadDataStepProcessor
    {
        public ReadDropboxStepProcessor()
        {
        }
        protected override void ReadData(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException(nameof(pipelineStep));
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException(nameof(pipelineContext));
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;
            //
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetDropboxSettings();
            if (settings == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.ApplicationName))
            {
                logger.Error(
                    "No application name is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.AccessToken))
            {
                logger.Error(
                    "No access token name is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }

            if (string.IsNullOrWhiteSpace(settings.RootPath))
            {
                logger.Error(
                    "No root path is specified on the endpoint. " +
                    "(pipeline step: {0}, endpoint: {1})",
                    pipelineStep.Name, endpoint.Name);
                return;
            }
 

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };


            var config = new DropboxClientConfig(settings.ApplicationName)
            {
                HttpClient = httpClient
            };

            var client = new DropboxClient(settings.AccessToken, config);

            var entries = client.Files.ListFolderAsync(settings.RootPath).Result.Entries.Where( e=> e.IsFile).ToList();
            var dropboxFiles = entries.Select(entry => new DropBoxFile(entry, settings));
        

            //
            //add the data that was read from the file to a plugin
            var dataSettings = new IterableDataSettings(dropboxFiles);
            logger.Info(
                "{0} rows were read from the file. (pipeline step: {1}, endpoint: {2})",
                entries.Count, pipelineStep.Name, endpoint.Name);
            //

            SitecoreItemUtilities sitecoreItemUtility = new SitecoreItemUtilities()
            {
                IsItemNameValid = (string x) => ItemUtil.IsItemNameValid(x),
                ProposeValidItemName = (string x) => ItemUtil.ProposeValidItemName(x)
            };

            Context.Plugins.Add(sitecoreItemUtility);

            //add the plugin to the pipeline context
            pipelineContext.Plugins.Add(dataSettings);
        }

       
    }
}
