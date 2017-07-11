using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DropboxProvider.Helpers;
using DropboxProvider.Models;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    [SupportedIds(new string[] { "{2DF31802-0AD1-41DC-B369-8854116A2B16}" })]
    public class ResolveDropboxFileStepConverter : BaseResolveObjectFromEndpointStepConverter
    {

        public ResolveDropboxFileStepConverter(IItemModelRepository repository) : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            base.AddPlugins(source, pipelineStep);
            this.AddCreateSitecoreItemSettings(source, pipelineStep);
            this.AddEndpointSettings(source, pipelineStep);
        }

        private void AddCreateSitecoreItemSettings(ItemModel source, PipelineStep step)
        {
            var resolveDropboxFileSettings = new ResolveDropboxFileSettings
            {
                MatchingFieldValueAccessor =
                    this.ConvertReferenceToModel<IValueAccessor>(source, "MatchingFieldValueAccessor"),
                ItemNameValueAccessor = 
                    this.ConvertReferenceToModel<IValueAccessor>(source, "ItemNameValueAccessor"),
            };


            step.Plugins.Add((IPlugin)resolveDropboxFileSettings);
        }

        private void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new EndpointSettings();
            var endpointTo = base.ConvertReferenceToModel<Endpoint>(source, "Endpoint To");
            if (endpointTo != null)
            {
                var dropboxSettings = endpointTo.GetDropboxSettings();
                pipelineStep.Plugins.Add(dropboxSettings);
            }

        }
    }
}
