using System;
using System.Collections.Generic;
using Dropbox.Api.Files;
using DropboxProvider.Helpers;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.Data;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Processors
{
    [RequiredPipelineStepPlugins(new Type[] { typeof(ResolveDropboxFileSettings) })]
    public class ResolveDropboxFileStepProcessor : BaseResolveObjectFromEndpointStepProcessor<DropBoxFile>
    {

        protected override DropBoxFile ResolveObject(string identifierValue, Endpoint endpoint, PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            if (!this.CanProcess(pipelineStep, pipelineContext))
                return null;

            SynchronizationSettings synchronizationSettings =
                Sitecore.DataExchange.Extensions.PipelineContextExtensions.GetSynchronizationSettings(pipelineContext);


            var settings = pipelineStep.GetPlugin<DropboxSettings>();

            var itemModel = synchronizationSettings.Source as ItemModel;

            var itemId = itemModel[ItemModel.ItemID];

            var dropboxRepository = new DropBoxRepository();
            var file = new DropBoxFile(new Metadata(), settings);
            DataAccessContext context = new DataAccessContext();

            var resolveSettings = pipelineStep.GetPlugin<ResolveDropboxFileSettings>();

            file.FileName = (string)resolveSettings.ItemNameValueAccessor.ValueReader.Read(synchronizationSettings.Source, context).ReadValue;

            var metaData = dropboxRepository.GetMetadata(file);
            if (metaData == null)
            {
                var customItemRepository = new CustomItemRepository();
                file.FileStream = customItemRepository.GetMediaItemFileStream(itemModel);

                var result = dropboxRepository.Update(file).Result;
                return file;
            }
            return null;
        }

    }
}

