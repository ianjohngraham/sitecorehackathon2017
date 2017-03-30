using Sitecore.Data;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Reflection;
using Sitecore.Resources.Media;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider.FieldWriter
{
    public class MediaItemFileStreamWriter : IValueWriter
    {
        public object[] Indexes
        {
            get;
            private set;
        }

        public IReflectionUtil ReflectionUtil
        {
            get;
            set;
        }

        public MediaItemFileStreamWriter(params object[] indexes)
        {
            this.Indexes = indexes;
            this.ReflectionUtil = new ReflectionUtil();
        }

        public virtual CanWriteResult CanWrite(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
           
     
            return new CanWriteResult()
            {
                CanWriteValue = true,
                IsGuess = false
            };
        }

        public static Stream GenerateStreamFromByteArray(byte[] s)
        {
            MemoryStream stream = new MemoryStream(s);
            //StreamWriter writer = new StreamWriter(stream);
            //writer.Write(s);
            //writer.Flush();
            //stream.Position = 0;
            
            return stream;
        }

        private PropertyInfo GetIndexerPropertySetter(object target, object value)
        {
            PropertyInfo indexerProperty = this.ReflectionUtil.GetIndexerProperty(target, this.Indexes);
            if (indexerProperty != null && indexerProperty.CanWrite)
            {
                ParameterInfo[] parameters = indexerProperty.GetSetMethod().GetParameters();
                if (!parameters.Any<ParameterInfo>())
                {
                    return null;
                }
                Type parameterType = parameters.LastOrDefault<ParameterInfo>().ParameterType;
                if (value == null)
                {
                    if (parameterType.IsValueType)
                    {
                        return null;
                    }
                    return indexerProperty;
                }
                if (parameterType.IsAssignableFrom(value.GetType()))
                {
                    return indexerProperty;
                }
            }
            return null;
        }

        public virtual bool Write(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            using (var stream = GenerateStreamFromByteArray(value as byte[]))
            {
                var creator = new MediaCreator();
                var itemId = ((ItemModel)target)[ItemModel.ItemID];

                var options = new MediaCreatorOptions();
                options.FileBased = false;
                options.IncludeExtensionInItemName = false;
                options.OverwriteExisting = true;
                options.Versioned = false;

                var mediaItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(ID.Parse(itemId));
                if (mediaItem != null)
                {

                    var updatedItem = creator.AttachStreamToMediaItem(stream, mediaItem.Paths.FullPath, mediaItem.Name,
                        options);

                    updatedItem.Editing.BeginEdit();
                    if (updatedItem.Fields["Extension"] != null)
                    {
                        updatedItem.Fields["Extension"].Value = "png";
                    }

                    updatedItem.Editing.EndEdit();

                }
                return true;
            }
        }
    }
}
