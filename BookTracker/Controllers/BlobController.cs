using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BookTracker.Repository;


namespace BookTracker.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobStorageRepository repo;

        public BlobController(IBlobStorageRepository _repo)
        {
            this.repo = _repo;
        }

        // GET: Blob
        public ActionResult Index(string journalID)
        {
            var blobVM = repo.GetBlobs("123");
            return View(blobVM);
        }

        public JsonResult JournalFiles(string journalID)
        {
            var blobVM = repo.GetBlobs(journalID);
            return Json(new { data = blobVM }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveBlob(string file, string extension, string journalID)
        {
            bool isDeleted = repo.DeleteBlob(file, extension, journalID);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UploadBlob(string journalID)
        {
            bool isSavedSuccessfully = true;
            string fName = "";

            try
            {
                foreach(string fileName in Request.Files)
                {
                    HttpPostedFileBase uploadFileName = Request.Files[fileName];
                    fName = uploadFileName.FileName;
                    bool isUploaded = repo.UploadBlob(uploadFileName, journalID);
                }
            }

            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }
        }
    }
}