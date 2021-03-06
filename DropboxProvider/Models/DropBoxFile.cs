﻿using System;
using System.IO;
using Dropbox.Api.Files;

namespace DropboxProvider.Models
{
   public  class DropBoxFile 
   {
        public DropBoxFile(Metadata metaData, DropboxSettings settings)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            Settings = settings;
            MetaData = metaData;
        }

        public DropboxSettings Settings { get; }
        public Metadata MetaData { get; }

        public Stream FileStream { get; set; }
        public  string FileName { get; set; }
   }
}
