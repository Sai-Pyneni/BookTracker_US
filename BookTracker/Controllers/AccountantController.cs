using BookTracker.Models;
using BookTracker.ViewModel;
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
using System.IO;
using System.Configuration;
using System.Globalization;

namespace BookTracker.Controllers
{
    public class AccountantController : Controller
    {
       private BT_Entity1 db = new BT_Entity1();
        // GET: Accountant


        public ActionResult Index()
        {           
           
            return View();
        }


        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Entry()
        {
            List<accountsTable> accountsList = db.accountsTables.Where(x => x.acctStateID == 1).ToList();
            ViewBag.ListofAccounts = new SelectList(accountsList, "acctNo", "acctName");
            List<journalTypesTable> journalTypesList = db.journalTypesTables.ToList();
            ViewBag.ListofJournalTypes = new SelectList(journalTypesList, "journalTypeID", "journalType");
            return View();

        }

        #region financial statements
        public ActionResult TrialBalance()
        {
            return View();
        }

        public JsonResult IncomeStatementExpense()
        {
            
            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 5 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();


                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                   
                    x.acctName,
                    x.totalDebits,
                 

                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult IncomeStatementRevenues()
        {
           

            using (db)
            {
                var data = db.accountsTables.Where(a=> a.acctCat == 4 && (a.totalDebits != 0 || a.totalCredits != 0 )).ToList();

                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();
                
                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    
                  
                    x.acctName,
                   x.totalCredits

                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult IncomeStatementTotals()
        {


            using (db)
            {
                decimal total = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                if(total == 0)
                {
                    foreach (var r in revs)
                    {
                        total -= r.balance;
                    }
                    foreach (var r in exps)
                    {
                        total -= r.balance;
                    }
                }


                var col = revs.Select(x => new
                {
                    total
                }
                );

                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =


                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult IncomeStatementTotalT()
        {


            using (db)
            {
                decimal total = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 ).ToList();
                if (total == 0)
                {
                    foreach (var r in revs)
                    {
                        total -= r.balance;
                    }
                    foreach (var r in exps)
                    {
                        total -= r.balance;
                    }
                }


                var col = revs.Select(x => new
                {
                    total
                }
                );

                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =


                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SoRE()
        {
            ViewBag.title = "BookTracker Accountants";
            ViewBag.statement = "Statement of Retained Earnings";
            ViewBag.date = DateTime.Now;

            using (db)
            {
             
                decimal iniBal = 0;
        
                var retEarnings = db.accountsTables.Where(a => a.acctNo == 600).ToList();
                

                foreach (var v in retEarnings)
                {
                    iniBal = v.initialBalace;
                }
                var col = retEarnings.Select(x => new
                {
                  x.initialBalace
                }
                );                      

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SoREDeb()
        {
           
            using (db)
            {
                var retEarnings = db.accountsTables.Where(a => a.acctNo == 600).ToList();            
              
                
                var col = retEarnings.Select(x => new
                {
                    x.totalDebits
                }
                );

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SoREBal()
        {

            using (db)
            {
                var retEarnings = db.accountsTables.Where(a => a.acctNo == 600).ToList();
                decimal balance = 0;
                foreach (var v in retEarnings)
                {
                    balance = v.initialBalace-v.totalDebits;
                }
                if(balance == 0)
                {
                    decimal total = 0;
                    var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                    var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                    if (total == 0)
                    {
                        foreach (var r in revs)
                        {
                            total -= r.balance;
                        }
                        foreach (var r in exps)
                        {
                            total -= r.balance;
                        }
                    }
                    balance = total;
                }

                var col = retEarnings.Select(x => new
                {
                    balance
                }
                );

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult IncomeStatement()
        {
            ViewBag.title = "BookTracker Accountants";
            ViewBag.statement = "Income Statement";
            ViewBag.date = DateTime.Now.Month + DateTime.Now.Year;
            return View();
        }

        public ActionResult BalanceSheet()
        {
            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                // var total = "Total Assets";
                var data2 = db.accountsTables.Where(a => (a.acctCat == 2 || a.acctCat == 3 || a.acctNo == 700) && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();

                decimal sumTotal = 0;
                decimal sumTotal2 = 0;
                if (sumTotal == 0)
                {
                    foreach (var i in data)
                    {

                        if (i.totalDebits > i.totalCredits)
                        {
                            sumTotal -= (i.totalDebits - i.totalCredits)*-1;
                            ///sumTotal = totalDebits;
                        }
                        else
                        {
                            sumTotal -= (i.totalDebits - i.totalCredits)*-1;
                            //sumTotal = totalCredits;
                        }
                    }
                }
                ViewBag.sumTotal = sumTotal.ToString("C",CultureInfo.CurrentCulture);
                
                if (sumTotal2 == 0)
                {
                    foreach (var i in data2)
                    {

                        if (i.totalDebits > i.totalCredits)
                        {
                            sumTotal2 += (i.totalDebits - i.totalCredits)*-1;
                            ///sumTotal = totalDebits;
                        }
                        else
                        {
                            sumTotal2 += i.totalCredits - i.totalDebits;
                            //sumTotal = totalCredits;
                        }                    }
                }
                ViewBag.sumTotal2 = sumTotal2.ToString("C", CultureInfo.CurrentCulture);
            }
          
            ViewBag.title = "BookTracker Accountants";
            ViewBag.statement = "Balance Sheet";
            ViewBag.date = DateTime.Now;
            return View();
        }

        public ActionResult StatementOfRetaindEarnings()
        {
            ViewBag.title = "BookTracker Accountants";
            ViewBag.statement = "Statement of Retained Earnings";
            ViewBag.date = DateTime.Now;
            return View();
        }

        public ActionResult EventLog()
        {
            return View();
        }
        #endregion
        /*
        public JsonResult CoAView()
        {
            using (db)
            {

                var data = db.accountsTables.OrderBy(a => a.acctNo).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
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
            }
        }

        */
       
       

        /*
        [HttpPost]
        public JsonResult LoadData()
        {
            using (db)
            {
                var data = db.journalTables.OrderBy(a => a.journalID).ToList();

                
                var col = data.Select(x => new {
                   x.journalID,
                    x.datePrepared,
                    x.journalTypesTable.journalType,
                    x.preparorID,
                    x.transactionStatusTable.transStatus,
                    totalCredits = getCreditDebit(x.journalID, 2),
                    totalDebits = getCreditDebit(x.journalID, 1)


                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult LoadTransactionCred(int journalId)
        {
            using (db)
            {
                var data = db.transactionEntryTables.Where(a => a.journalID == journalId && a.transactionType == 1).ToList();
                data.OrderBy(a => a.acctNo);

                var col = data.Select(x => new {   
                   
                    x.accountsTable.acctName,
                   x.transactionTypeTable.transactionType,
                   
                    x.transactionAmount,
                }).ToList();
                return Json(new { data = col }, JsonRequestBehavior.AllowGet);

            }
          
        }
        public JsonResult LoadTransactionDeb(int journalId)
        {
            using (db)
            {
                var data = db.transactionEntryTables.Where(a => a.journalID == journalId && a.transactionType == 2).ToList();
                data.OrderBy(a => a.acctNo);

                var col = data.Select(x => new {

                    x.accountsTable.acctName,
                    x.transactionTypeTable.transactionType,

                    x.transactionAmount,
                }).ToList();
                return Json(new { data = col }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult SaveTransactions(string date, string journalType, string comments, List<Transactions>  transactions)
        {
            

            string result = "Error! Transaction Is Not Complete!";
            string jID = "";
            if (date != null && journalType != null && transactions != null)
            {
                // var journalID = Guid.NewGuid();
                journalTable journal = new journalTable();
                //journal.journalID = journalID;
                journal.preparorID = (int)Session["userID"];
                journal.approverID = (int)Session["userID"];
                journal.comments = comments.ToString();
                journal.datePrepared = DateTime.Parse(date);
                journal.isAttach = 0;
                journal.journalStatus = 0;
                journal.reason = "";
                journal.journalTypeID = int.Parse(journalType);
                db.journalTables.Add(journal);


                foreach (var item in transactions)
                {

                    transactionEntryTable entryTable = new transactionEntryTable();
                    entryTable.acctNo = item.acctNo;
                    entryTable.journalID = journal.journalID;
                    //entryTable.transStatusID = 0;
                    entryTable.transactionAmount = item.transactionAmount;
                    entryTable.transactionType = item.transactionType;
                    db.transactionEntryTables.Add(entryTable);
                    // entryTable.transStatusID = item.
                    //entryTable = transactions;

                    ledgerTable ledgerTable = new ledgerTable();
                    ledgerTable.journalID = journal.journalID;
                    ledgerTable.approvalDate = journal.datePrepared;
                    ledgerTable.acctNo = entryTable.acctNo;
                    if (entryTable.transactionType == 2)
                    {
                        ledgerTable.debits = (decimal)entryTable.transactionAmount;
                        //ledgerTable.credits = 0;
                        ledgerTable.balance += (decimal)ledgerTable.debits;
                    }
                    else if (entryTable.transactionType == 1)
                    {
                        ledgerTable.credits = (decimal)entryTable.transactionAmount;
                        //ledgerTable.debits = 0;
                        ledgerTable.balance -= (decimal)ledgerTable.credits;
                    }
                    db.ledgerTables.Add(ledgerTable);

                    eventLog log = new eventLog();
                    log.userID = journal.preparorID;
                    log.transactionID = entryTable.transactionID;
                    log.journalID = journal.journalID;

                    jID = journal.journalID.ToString();

                    log.fromPage = "New Transaction Entered, Transaction Type: " + entryTable.transactionType;
                    log.toPage = "Account affected: " + entryTable.acctNo;
                    log.eventDate = DateTime.Now;
                    db.eventLogs.Add(log);
                    db.SaveChanges();
                    result = "Success! Transaction Is Complete!";
                }
            }

            var resultID = Json(new { message = result, journalID = jID });

            return Json(resultID, JsonRequestBehavior.AllowGet);
        }
        */


        /*
        public JsonResult TrialBalanceView()
        {
            ViewBag.title = "BookTracker Accountants";
            ViewBag.statement = "Trial Balance";
            ViewBag.date = DateTime.Now.Month + DateTime.Now.Year;

            using (db)
            {
                var data = db.accountsTables.Where(a => a.totalDebits != 0 || a.totalCredits != 0).ToList();

               
                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();
            
                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.acctNo,
                    x.acctName,    
                    
                   x.totalDebits,
                   x.totalCredits
                  
                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }
               
        }*/



        /*
        public decimal getCreditDebit(int journalID, int transactionTypeID)
        {

            var transactions = db.transactionEntryTables.Where(a => a.journalID == journalID).ToList();

            decimal total = 0;

            foreach (var t in transactions)
            {
                if (t.transactionType == transactionTypeID)
                    total += t.transactionAmount;
            }

            return total;
        }



        //ledger details view details
        [HttpPost]
        public JsonResult LoadLedger(int acctNo)
        {
            using (db)
            {
                
                var data = db.ledgerTables.Where(a => a.acctNo == acctNo && a.lStatus == 1 ).ToList();
                ViewBag.acctNo = acctNo.ToString(); 

                var col = data.Select(x => new
                {                
                    
                    x.approvalDate,                  
                    x.journalID,                    
                    x.debits,
                    x.credits,
                }).ToList();

                return Json(new { data = col  }, JsonRequestBehavior.AllowGet);

            }
        }

    */


    }


}