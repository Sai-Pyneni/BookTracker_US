using BookTracker.Models;
using BookTracker.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookTracker.Controllers
{
    public class AcMnEventLogController : Controller
    {
        private BT_Entity1 database = new BT_Entity1();
        // GET: AcMnEventLog
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult LoadData()
        {
            using (database)
            {

                var data = database.eventLogs.Where(a => a.journalID > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.eventID,
                    x.eventDate,
                    x.fromPage,
                    x.toPage,
                    x.journalID,
                    x.userID,
                    x.userID2,

                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}