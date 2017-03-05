using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider
{
    public class DropboxEndpointConverter : BaseEndpointConverter<ItemModel>
    {
        private static readonly Guid TemplateId = Guid.Parse("{E82930AE-5537-4504-A12B-58DFAE962CB2}");
        public DropboxEndpointConverter(IItemModelRepository repository) : base(repository)
        {
            //
            //identify the template an item must be based
            //on in order for the converter to be able to
            //convert the item
            this.SupportedTemplateIds.Add(TemplateId);
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
            //
            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}
