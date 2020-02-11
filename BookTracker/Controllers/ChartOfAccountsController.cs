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
using System.Linq.Dynamic;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace BookTracker.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private BT_Entity1 database2 = new BT_Entity1();

        // GET: Admin
        public ActionResult Index()
        {

           // updateBalance();
           

            List<accountCategoryTable> catList = database2.accountCategoryTables.ToList();
            ViewBag.ListOfCat = new SelectList(catList,"acctCatID", "catName");
            List<accountSubCategoryTable> subCatList = database2.accountSubCategoryTables.ToList();
            ViewBag.ListofSubCat = new SelectList(subCatList, "acctSubCatID", "acctSubCatName");
            List<accountTermTable> termList = database2.accountTermTables.ToList();
            ViewBag.ListofTerm = new SelectList(termList, "acctTermID", "termName");
            List<accountStateTable> stateList = database2.accountStateTables.ToList();
            ViewBag.ListofState = new SelectList(stateList, "acctStateID", "actState");




            return View();
            #region COA list
            /*List<CharterAccountsModel> list = new List<CharterAccountsModel>();



            var v = (from a in database.accountsTables
                     join b in database.accountCategoryTables on a.acctCat equals b.acctCatID
                     join c in database.accountSubCategoryTables on a.acctSubCat equals c.acctSubCatID
                     join d in database.accountTermTables on a.acctTerm equals d.acctTermID
                     join e in database.accountStateTables on a.acctStateID equals e.acctStateID
                     select new CharterAccountsModel
                     {
                         acctNo = a.acctNo,
                         acctName = a.acctName,
                         acctCatName = b.catName,
                         acctSubCatName = c.acctSubCatName,
                         termName = d.termName,
                         balance = a.balance,
                         dateCreated = (DateTime)a.dateCreated,
                         createdBy = (int)a.createdBy,
                         editedBy = (int)a.editedBy,
                         acctStateName = (int)a.acctStateID,
                         comments = a.comments,



                     });
            list = v.ToList();
            return View(list);*/
            #endregion



        }
        [HttpPost]        
        public JsonResult LoadData()
        {
            #region comments for old code
            //jQuery DataTable Param
            /* var draw = Request.Form.GetValues("draw").FirstOrDefault();
              //find paging info
              var start = Request.Form.GetValues("start").FirstOrDefault();
              var length = Request.Form.GetValues("length").FirstOrDefault();
              //Find order column
              var sortCol = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault()
                  +"][name]").FirstOrDefault();
              var sortColDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
              //find search column

              var acctNo =  Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
              var acctCat = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();

              int pageSize = length != null ? Convert.ToInt32(length) : 0;
              int skip = start != null ? Convert.ToInt16(start) : 0;
              int recordsTot = 0; */
            #endregion
            using (database2)
                {
                
                var data = database2.accountsTables.OrderBy(a => a.acctNo).ToList();
                
                //List<accountsTable> accounts =
                var col = data.Select(x => new {
                    x.acctNo,
                    x.acctName,
                    x.accountCategoryTable.catName,
                    x.accountSubCategoryTable.acctSubCatName,
                    x.balance,
                    x.createdBy,
                    x.dateCreated,
                    x.accountStateTable.actState,
                    x.comments,                    
                    x.accountTermTable.termName                   
                    }).ToList();
                
                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
                #region try
                //Searching...
                /*if (!string.IsNullOrEmpty(acctNo))
                {
                    v = v.Where(a => a.acctNo.Equals(acctNo));
                }
                if (!string.IsNullOrEmpty(acctCat))
                {
                    v = v.Where(a => a.acctCat.Equals(acctCat));
                }
                //Sorting..
                if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortColDir)))
                {
                    v = v.OrderBy(sortCol + " " + sortColDir);
                }
                recordsTot = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList(); 
                 return Json(new {draw= draw, recordsFiltered = recordsTot, recordsTot= recordsTot, data= data },
                    JsonRequestBehavior.AllowGet);
*/

                //return Json(new {draw= draw, recordsFiltered = recordsTot, recordsTot= recordsTot, data= data },JsonRequestBehavior.AllowGet);
#endregion

            }

        }
     
        public ActionResult GetAccountByNo(int acctNo)
        {
            var data = database2.accountsTables.Where(a => a.acctNo == acctNo);
            
            var col = data.Select(x => new {
                x.acctNo,
                x.acctName,
                x.accountCategoryTable.catName,
                x.accountSubCategoryTable.acctSubCatName,             
                x.accountStateTable.actState,             
                x.accountTermTable.termName
            }).ToList();
        
            return Json(col, JsonRequestBehavior.AllowGet);
       
        }

        [HttpPost]
        public JsonResult DeactivateByAcctNo(int acctNo)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (database2)
            {
                var account = database2.accountsTables.Where(x => x.acctNo == acctNo).FirstOrDefault();
                if (account.acctStateID == 1)
                {
                    account.acctStateID = 0;
                }
                else account.acctStateID = 1;

                eventLog eventLog = new eventLog();
                eventLog.fromPage = "Deactivated/Activated Account: " + acctNo;
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Account State: " + account.acctStateID;
                eventLog.userID = (int)Session["userID"];
                eventLog.eventDate = DateTime.Now;
                database2.eventLogs.Add(eventLog);
                database2.SaveChanges();
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
           

            using (database2)
            {
                //userTable user = new userTable();
                //user.userID = (int)Session["userID"];
                //ViewBag.acctCat = new SelectList(database2.accountsTables, "acctCatID","catName");
                ViewBag.catList = new SelectList(database2.accountCategoryTables.ToList().Select(x => new { acctCatID = x.acctCatID, acct = x.catName }), "acctCatID", "acctName");
                List<accountCategoryTable> catList = database2.accountCategoryTables.ToList();
                ViewBag.ListOfCat = new SelectList(catList, "acctCatID", "catName");
                List<accountSubCategoryTable> subCatList = database2.accountSubCategoryTables.ToList();
                ViewBag.ListofSubCat = new SelectList(subCatList, "acctSubCatID", "acctSubCatName");
                List<accountTermTable> termList = database2.accountTermTables.ToList();
                ViewBag.ListofTerm = new SelectList(termList, "acctTermID", "termName");
                List<accountStateTable> stateList = database2.accountStateTables.ToList();
                ViewBag.ListofState = new SelectList(stateList, "acctStateID", "actState");

                return View(database2.accountsTables.Where(x => x.acctNo == id).FirstOrDefault());

            }
        }

        [HttpPost]
        public ActionResult Edit(int id, accountsTable model)
        {

            //var currentAccount = database2.accountsTables.Where(p => p.acctNo = model.acctNo)F;
            // userTable u = new userTable();
            //u.userID = user.userID;
            using (database2)
            {
                eventLog log = new eventLog();


                DateTime dt = DateTime.Now;
                log.eventDate = dt;
                log.fromPage = "Account: " + model.acctNo + "Status: " + model.acctStateID + "Category: " + model.acctCat + "Sub-Category: " + model.acctSubCat;
                log.userID = model.createdBy;
                model.editedBy = (int)Session["userID"];

                var v = database2.accountsTables.SingleOrDefault(a => a.acctNo == id);

                //model.createdBy = (int)Session["createdBy"];
                //model.dateCreated = (DateTime)Session["dateCreated"];
                model.acctNo = v.acctNo;
                //model.acctName = model.acctName;

                model.totalCredits = v.totalCredits;
                model.totalDebits = v.totalDebits;
                model.balance = v.balance;
                model.initialBalace = v.initialBalace;
                model.createdBy = v.createdBy;
                model.editedBy = (int)Session["userID"];
               
                v.editedBy = (int)Session["userID"];


                database2.Set<accountsTable>().AddOrUpdate(model);

                log.userID2 = v.editedBy;
                log.toPage = "Account: " + v.acctNo + "Status: " + v.acctStateID + "Category: " + v.acctCat + "Sub-Category: " + v.acctSubCat;
                database2.eventLogs.Add(log);
                //database2.Entry(model).State = EntityState.Modified;
                database2.SaveChanges();





                return RedirectToAction("Index");

            }
            // TODO: Add update logic here




        }
        [HttpPost]
        public JsonResult EditDatainDB(CharterAccountsModel model)
        {
            var result = false;
            try
            {

                accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == model.acctNo);
                accounts.acctNo = model.acctNo;
                accounts.acctName = model.acctName;
                accounts.acctStateID = model.acctStateID;
                accounts.acctCat = model.acctCat;
                accounts.acctSubCat = model.acctSubCat;
                accounts.acctTerm = model.acctTerm;
                accounts.editedBy = (int)Session["userID"];
                database2.Entry(model).State = EntityState.Modified;
                database2.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]        
        public JsonResult SaveDataInDB(CharterAccountsModel model)
        {
            var result = false;
          
            try
            {
                if (ModelState.IsValid)
                {
                    accountsTable accounts = new accountsTable();

                    accounts.acctSubCat = model.acctSubCat;
                    accounts.acctNo = model.acctNo;

                    accounts.acctCat = model.acctCat;
                    accounts.acctStateID = model.acctStateID;
                    accounts.acctName = model.acctName;
                    accounts.totalCredits = (decimal)0;
                    accounts.totalDebits = (decimal)0;
                    accounts.balance = (decimal)0;
                    accounts.createdBy = (int)Session["userID"];
                    accounts.dateCreated = DateTime.Today;
                    accounts.acctTerm = model.acctTerm;

                    database2.accountsTables.Add(accounts);
                    

                    eventLog eventLog = new eventLog();
                    eventLog.fromPage = "Create Account";
                    eventLog.toPage = "Create Account";
                    eventLog.userID = (int)Session["userID"];
                    eventLog.attachID = 0;
                    eventLog.eventDate = DateTime.Now;
                    eventLog.journalID = 0;
                    eventLog.transactionID = 0;
                    eventLog.ledgerID = 0;
                    eventLog.userID2 = (int)Session["userID"];
                    database2.eventLogs.Add(eventLog);
                    database2.SaveChanges();
                    result = true;
                }               
                //}

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(result,
                 
                JsonRequestBehavior.AllowGet);
        }
        private void updateBalance()
        {
            var transations = database2.transactionEntryTables.OrderBy(a => a.transactionID).ToList();
            var accounts = database2.accountsTables.OrderBy(a => a.acctNo).ToList();

            foreach (var a in accounts)
            {
                a.totalCredits = 0;
                a.totalDebits = 0;

                foreach (var t in transations)
                {
                    if (t.acctNo == a.acctNo)
                    {
                        if (t.transactionType == 1)
                        {
                            a.totalCredits += t.transactionAmount;
                        }
                        else
                        {
                            a.totalDebits += t.transactionAmount;
                        }
                    }
                }
                a.balance = a.initialBalace + a.totalCredits - a.totalDebits;
                database2.SaveChanges();
            }
        }
       
           


    }
}