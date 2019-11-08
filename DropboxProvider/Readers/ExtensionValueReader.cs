using System;
using Sitecore.DataExchange.DataAccess;

namespace DropboxProvider.Readers
{
    public class ExtensionValueReader : IValueReader
    {

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (context == null)
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }

            if (string.IsNullOrWhiteSpace(((string)source)))
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }
           
            string stringVal = System.IO.Path.GetExtension((string)source).Replace(".", "");

            return ReadResult.PositiveResult(stringVal, DateTime.Now);
        }
    }
}

