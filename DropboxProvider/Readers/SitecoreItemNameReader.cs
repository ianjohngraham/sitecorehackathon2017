using System;
using DropboxProvider.Models;
using Sitecore.DataExchange.DataAccess;
using Sitecore.Services.Core.Model;

namespace DropboxProvider.Readers
{
    public class SitecoreItemNameReader : IValueReader
    {
        public SitecoreItemNameReader()
        {
   
        }

        public string FieldName { get; private set; }

        private bool IncludeExtension { get; set; }

        public SitecoreItemNameReader(bool includeExtension)
        {
            IncludeExtension = includeExtension;
        }

        public CanReadResult CanRead(object source, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.FieldName = "ItemName";

            var itemModel = (ItemModel) source;

            return new CanReadResult()
            {
                CanReadValue = itemModel.ContainsKey("ItemName") && itemModel.ContainsKey("Extension")
            };
        }

        public ReadResult Read(object source, DataAccessContext context)
        {
            string returnValue = string.Empty;
            if (IncludeExtension)
            {
                 returnValue = ((ItemModel) source)["ItemName"].ToString() + "." +
                                ((ItemModel) source)["Extension"].ToString();
            }
            else
            {
                returnValue = ((ItemModel) source)["ItemName"].ToString();
            }
            return new ReadResult(DateTime.UtcNow) { ReadValue = returnValue, WasValueRead = true };
        }
    }
}
