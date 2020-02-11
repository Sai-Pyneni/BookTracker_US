using BookTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookTracker.Controllers
{
    public class AttachmentsController : Controller
    {
        private BT_Entity1 db = new BT_Entity1();
        // GET: Attachments
        public ActionResult Index()
        {
            return View();
        }

        // UploadFiles works fine, may need refining for folder location
        [HttpPost]
        public ActionResult UploadFiles(string journalID)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;

                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Uploads/journalFiles/" + journalID));
                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);



                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);

                        attachTable attachements = new attachTable();
                        attachements.journalID = Int32.Parse(journalID);
                        attachements.attachLocation = ("/Uploads/" + journalID + "/" + file.FileName);

                        db.attachTables.Add(attachements);

                        db.SaveChanges();
                    }
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

        public JsonResult loadFiles(int journalId)
        {
            using (db)
            {
                var data = db.attachTables.Where(a => a.journalID == journalId).ToList();
                data.OrderBy(a => a.attachID);

                var cols = data.Select(x => new
                {
                    x.attachLocation,
                    x.journalID,

                }).ToList();

                if (cols.Count() <= 0)
                {
                    var message = "no attachments";
                    return Json(new { attachLocation = message, journalID = journalId });
                }

                else

                    return Json(new { data = cols }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}