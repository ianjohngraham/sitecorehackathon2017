using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.DataExchange.Local.Repositories;
using Sitecore.Resources.Media;
using Sitecore.Services.Core.Model;
using Sitecore.Services.Infrastructure.Model;
using Sitecore.Services.Infrastructure.Sitecore.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider.Repository
{
    public class CustomItemRepository : InProcItemModelRepository
    {
        public override Guid Create(string itemName, Guid templateId, Guid parentId, string language)
        {
            var parentItem = base.Get(parentId);
            if (parentItem == null)
            {
                return Guid.Empty;
            }
            var path = parentItem[ItemModel.ItemPath].ToString();
            //
            var itemModel = new ItemModel();

            itemModel[ItemModel.ItemName] = itemName;
            itemModel[ItemModel.TemplateID] = templateId.ToString();
            var handler = base.HandlerProvider.GetHandler<CreateItemHandler>();
            var command = new CreateItemCommand
            {
                ItemModel = itemModel,
                Path = path,
                Language = language

            };
            var response = handler.Handle(command) as CreateItemResponse;
         

            return response.ItemId;
        }

        public static MemoryStream GenerateStreamFromByteArray(byte[] s)
        {
            MemoryStream stream = new MemoryStream(s);
            return stream;
        }

        public Stream GetMediaItemFileStream(ItemModel itemModel)
        {
            var mediaItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(ID.Parse(itemModel[ItemModel.ItemID]));
            if (mediaItem != null)
            {
                var stream = mediaItem.Fields["Blob"].GetBlobStream();
                return stream;
            }
            return null;
        }

        private void UpdateMediaItem(ItemModel itemModel)
        {
            var bytes =(byte[]) Convert.FromBase64String((string)itemModel["Blob"]);

            using (var stream = GenerateStreamFromByteArray(bytes))
            {
                var creator = new MediaCreator();
                var itemId = itemModel[ItemModel.ItemID];

                var options = new MediaCreatorOptions();
                options.FileBased = false;
                options.IncludeExtensionInItemName = false;
                options.OverwriteExisting = true;
                options.Versioned = false;

                var mediaItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(ID.Parse(itemId));
                if (mediaItem != null)
                {
                    mediaItem.Editing.BeginEdit();
                    if (mediaItem.Fields["Blob"] != null)
                    {
                        mediaItem.Fields["Blob"].SetBlobStream(stream);
                    }
                    mediaItem.Editing.EndEdit();
                }
            }
        }

        public override bool Update(Guid id, ItemModel itemModel, string language, int version)
        {
            var handler = base.HandlerProvider.GetHandler<UpdateItemHandler>();

            var command = new UpdateItemCommand
            {
                Id = id,
                Database = base.DatabaseName,
                ItemModel = itemModel,
                Language = language,
                Version = version.ToString()
            };
            handler.Handle(command);

            if (itemModel.ContainsKey("Blob"))
            {
                UpdateMediaItem(itemModel);
            }
            return true;
        }
    }
}

