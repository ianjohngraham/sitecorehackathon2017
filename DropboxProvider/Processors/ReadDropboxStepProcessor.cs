﻿using System;
using System.Linq;
using System.Net.Http;
using Dropbox.Api;
using DropboxProvider.Helpers;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.Data.Items;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.Plugins;

namespace DropboxProvider.Processors
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
 
            var dropboxRepository = new DropBoxRepository();

            var dropboxFiles = dropboxRepository.ReadAll(settings);

            //
            //add the data that was read from the file to a plugin
            var dataSettings = new IterableDataSettings(dropboxFiles);
            logger.Info(
                "{0} rows were read from the file. (pipeline step: {1}, endpoint: {2})",
                dropboxFiles.Count(), pipelineStep.Name, endpoint.Name);
            

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
