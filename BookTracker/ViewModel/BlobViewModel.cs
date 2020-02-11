using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace BookTracker.ViewModel
{
    public class BlobViewModel
    {
        public string BlobContainerName { get; set; }
        public string StorageUri { get; set; }
        public string ActualFileName { get; set; }
        public string PrimaryUri { get; set; }
        public string fileExtension { get; set; }

        public string FileNameWithoutExt
        {
            get
            {
                return Path.GetFileNameWithoutExtension(ActualFileName);
            }
        }

        public string FileNameExtensionOnly
        {
            get
            {
                return Path.GetExtension(ActualFileName).Substring(1);
            }
        }
    }
}