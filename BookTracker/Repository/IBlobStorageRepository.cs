using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookTracker.ViewModel;

namespace BookTracker.Repository
{
    public interface IBlobStorageRepository
    {

        IEnumerable<BlobViewModel> GetBlobs(string journalID);

        bool DeleteBlob(string file, string fileExtension, string journalID);
        bool UploadBlob(HttpPostedFileBase blobFile, string journalID);
        

    }
}