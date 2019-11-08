using System;
using DropboxProvider.Models;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Converters
{
    [SupportedIds("{E82930AE-5537-4504-A12B-58DFAE962CB2}")]
    public class DropboxEndpointConverter : BaseEndpointConverter
    {
        public DropboxEndpointConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //
            //create the plugin
            var settings = new DropboxSettings();
            //
            //populate the plugin using values from the item
            settings.ApplicationName =
                base.GetStringValue(source, DropboxEndpointItemModel.ApplicationName);
            settings.AccessToken =
                base.GetStringValue(source, DropboxEndpointItemModel.AccessToken);

            settings.RootPath =
                base.GetStringValue(source, DropboxEndpointItemModel.RootPath);

            //add the plugin to the endpoint
            endpoint.AddPlugin(settings);
        }
    }
}
