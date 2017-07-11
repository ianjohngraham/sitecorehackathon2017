using System;
using System.Collections.Generic;
using Dropbox.Api.Files;
using DropboxProvider.Helpers;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.Data;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Processors
{
    [RequiredEndpointPlugins(typeof(DropboxSettings))]
    public class UpdateDropboxFilePipelineStep : BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            if (!this.CanProcess(pipelineStep, pipelineContext))
                return;

            SynchronizationSettings synchronizationSettings = Sitecore.DataExchange.Extensions.PipelineContextExtensions.GetSynchronizationSettings(pipelineContext);

            EndpointSettings endpointSettings = Sitecore.DataExchange.Extensions.PipelineStepExtensions.GetEndpointSettings(pipelineStep);
            if (endpointSettings == null)
                return;

            var settings = endpointSettings.EndpointTo.GetDropboxSettings();

             var itemModel = synchronizationSettings.Source as ItemModel;

            var itemId = itemModel[ItemModel.ItemID];

            var mediaItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(ID.Parse(itemId));
            if (mediaItem != null)
            {

                var stream =  mediaItem.Fields["Blob"].GetBlobStream();

                DropBoxFile file = new DropBoxFile(new Metadata(), settings);
                file.FileName = "Test.png";
                file.FileStream = stream;
                var dropboxRepository = new DropBoxRepository();
                var result = dropboxRepository.Update(file).Result;
            }
        }

        private IItemModelRepository GetItemModelRepository(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            EndpointSettings endpointSettings = Sitecore.DataExchange.Extensions.PipelineStepExtensions.GetEndpointSettings(pipelineStep);
            if (endpointSettings == null)
                return (IItemModelRepository)null;
            Endpoint endpointTo = endpointSettings.EndpointTo;
            if (endpointTo == null)
                return (IItemModelRepository)null;
            ItemModelRepositorySettings repositorySettings = Sitecore.DataExchange.Providers.Sc.Extensions.EndpointExtensions.GetItemModelRepositorySettings(endpointTo);
            if (repositorySettings == null)
                return (IItemModelRepository)null;
            return repositorySettings.ItemModelRepository;
        }

     
    }
}
