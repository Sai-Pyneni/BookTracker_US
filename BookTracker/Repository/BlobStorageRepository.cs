using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookTracker.ViewModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace BookTracker.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private StorageCredentials _credential;
        private CloudStorageAccount _account;
        private CloudBlobContainer _container;
        private CloudBlobClient _client;
        private CloudBlobDirectory _directoy;

        private string containerName = "journal";
        

        public BlobStorageRepository()
        {
            string accountName = "booktrackerfiles";
            string key = "D4Rstmgm2xQdBwQFpMThMhSnigAWKxt+pWcPdVWOBEDbQFO+g3GA24fYaQ6DsdVWEL3Ta4BMsWqqDId0j0VwUg==";

            _credential = new StorageCredentials(accountName, key);
            _account = new CloudStorageAccount(_credential, true);
            _client = _account.CreateCloudBlobClient();
            _container = _client.GetContainerReference(containerName);
        }
        //working fine
        public bool DeleteBlob(string file, string fileExtension, string journalID)
        {
            _container = _client.GetContainerReference(containerName);
            _directoy = _container.GetDirectoryReference(journalID);

            CloudBlockBlob blockBlob = _directoy.GetBlockBlobReference(file + "." + fileExtension);
            bool deleted = blockBlob.DeleteIfExists();

            return deleted;
        }



        //working fine
        public IEnumerable<BlobViewModel> GetBlobs(string journalID)
        {

            _directoy = _container.GetDirectoryReference(journalID);
            var content = _directoy.ListBlobs().ToList();
            IEnumerable<BlobViewModel> VM = content.Select(x => new BlobViewModel
            {
                BlobContainerName = x.Container.Name,
                StorageUri = x.StorageUri.PrimaryUri.ToString(),
                PrimaryUri = x.StorageUri.PrimaryUri.ToString(),
                ActualFileName = x.Uri.AbsoluteUri.Substring(x.Uri.AbsoluteUri.LastIndexOf("/") + 1),
                fileExtension = Path.GetExtension(x.Uri.AbsoluteUri.Substring(x.Uri.AbsoluteUri.LastIndexOf("/") + 1)
                )
            }).ToList();

            return VM;
        }
        //Working fine
        public bool UploadBlob(HttpPostedFileBase blobFile, string journalID)
        {
            if(blobFile==null)
            {
                return false;
            }

            
            _container = _client.GetContainerReference(containerName);
            _directoy = _container.GetDirectoryReference(journalID);

            CloudBlockBlob blockBlob = _directoy.GetBlockBlobReference(blobFile.FileName.Replace(" ", string.Empty));
           

            using (var fileStream = (blobFile.InputStream))
            {
                try
                {
                    blockBlob.UploadFromStream(fileStream, accessCondition: AccessCondition.GenerateIfNoneMatchCondition("*"));
                }
                catch (StorageException )
                {
                    var addedName = 1;
                    while (blockBlob.Exists())
                    {
                        var newFileName = Path.GetFileNameWithoutExtension(blobFile.FileName) +"(" + (addedName) + ")." + Path.GetExtension(blobFile.FileName).Substring(1);
                        
                        blockBlob = _directoy.GetBlockBlobReference(newFileName.Replace(" ", string.Empty));
                        addedName++;
                    }

                    blockBlob.UploadFromStream(fileStream, accessCondition: AccessCondition.GenerateIfNoneMatchCondition("*"));

                }
                //blockBlob.UploadFromStream(fileStream);
            }
            return true;
        }
    }
}