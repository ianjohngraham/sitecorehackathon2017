using System;
using Dropbox.Api.Files;
using DropboxProvider.Models;
using DropboxProvider.Repository;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Processors
{
    [RequiredPipelineStepPlugins(new Type[] { typeof(ResolveDropboxFileSettings) })]
    public class ResolveDropboxFileStepProcessor : BaseResolveObjectFromEndpointStepProcessor<DropBoxFile>
    {
        ////TODO: Rewrite ResolveDropboxFileStepProcessor
        /// TODO: Is there any identifier of a Dropbox File?
        public override object FindExistingObject(DropBoxFile identifierValue, PipelineStep pipelineStep, PipelineContext pipelineContext,
            ILogger logger)
        {
            SynchronizationSettings synchronizationSettings =
                Sitecore.DataExchange.Extensions.PipelineContextExtensions.GetSynchronizationSettings(pipelineContext);


            var settings = pipelineStep.GetPlugin<DropboxSettings>();

            var itemModel = synchronizationSettings.Source as ItemModel;

            var itemId = itemModel[ItemModel.ItemID];

            var dropboxRepository = new DropBoxRepository();
            var file = new DropBoxFile(new Metadata(), settings);
            DataAccessContext context = new DataAccessContext();

            var resolveSettings = pipelineStep.GetPlugin<ResolveDropboxFileSettings>();
            ////TODO: Should not access Source directly - it should be configurable
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

        public override object CreateNewObject(DropBoxFile identifierValue, PipelineStep pipelineStep, PipelineContext pipelineContext,
            ILogger logger)
        {
            var settings = pipelineStep.GetPlugin<DropboxSettings>();
            var file = new DropBoxFile(new Metadata(), settings);
            return file;
        }

        protected override DropBoxFile ConvertValueToIdentifier(object identifierValue, PipelineStep pipelineStep, PipelineContext pipelineContext,
            ILogger logger)
        {

            var settings = pipelineStep.GetPlugin<DropboxSettings>();
            var file = new DropBoxFile(new Metadata(), settings);
            return file;
        }
    }
}

