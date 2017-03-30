using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Writers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropboxProvider.FieldWriter
{
    public class SitecoreMediaItemFieldWriter : IValueWriter
    {
        public string FieldName
        {
            get;
            private set;
        }

        public string Language
        {
            get;
            private set;
        }

        protected IValueWriter ValueWriter
        {
            get;
            set;
        }

        public SitecoreMediaItemFieldWriter(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentOutOfRangeException("fieldName", fieldName, "Field name must be specified.");
            }
            this.FieldName = fieldName;
        }

        public SitecoreMediaItemFieldWriter(string fieldName, string language) : this(fieldName)
        {
            this.Language = language;
        }

        public virtual CanWriteResult CanWrite(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.ValueWriter == null)
            {
                this.ValueWriter = this.CreateValueWriter();
            }
            return this.ValueWriter.CanWrite(target, value, context);
        }

        protected virtual IValueWriter CreateFieldValueWriter()
        {
            return new MediaItemFileStreamWriter(new object[] { this.FieldName });
        }

        protected virtual IValueWriter CreateLanguageValueWriter()
        {
            return new MediaItemFileStreamWriter(new object[] { CultureInfo.CreateSpecificCulture(this.Language), this.FieldName });
        }

        protected virtual IValueWriter CreateValueWriter()
        {
            if (string.IsNullOrEmpty(this.Language))
            {
                return this.CreateFieldValueWriter();
            }
            return this.CreateLanguageValueWriter();
        }

        public virtual bool Write(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this.ValueWriter == null)
            {
                this.ValueWriter = this.CreateValueWriter();
            }
            return this.ValueWriter.Write(target, value, context);
        }
    }
}
