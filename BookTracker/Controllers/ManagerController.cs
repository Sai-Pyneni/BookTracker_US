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
using System.Globalization;

namespace BookTracker.Controllers
{
    public class ManagerController : Controller
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

        [HttpPost]
        public JsonResult ApproveJournal(int journalID)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (db)
            {
                var journal = db.journalTables.Where(x => x.journalID == journalID).FirstOrDefault();
                var ledger = db.ledgerTables.Where(a => a.journalID == journalID).ToList();
               
                if (journal.journalStatus == 0)
                {
                    
                    foreach(var v in ledger)
                    {
                        v.lStatus = 1;
                    }
                    journal.journalStatus = 1;
                    journal.approverID = (int)Session["userID"];
                }
                else
                {
                    journal.journalStatus = 0;
                    foreach (var v in ledger)
                    {
                        v.lStatus = 0;
                    }
                    journal.approverID = (int)Session["userID"];
                }



                updateBalance(journal.journalID, 1);
                db.SaveChanges();
                decimal total = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                var incomeExpenseSummary = db.accountsTables.Where(a => a.acctNo == 700).SingleOrDefault();
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
                //incomeExpenseSummary
                incomeExpenseSummary.totalCredits = total;
                incomeExpenseSummary.balance = total;
                incomeExpenseSummary.totalDebits = 0;
               
                /*eventLog eventLog = new eventLog();
                eventLog.fromPage = "Journal" + journalID + "Approved";
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Account State:" + account.acctStateID;
                eventLog.userID = (int)Session["userID"];
                eventLog.eventDate = DateTime.Now;
                database2.eventLogs.Add(eventLog); */
                eventLog log = new eventLog();
                log.eventDate = DateTime.Now;
                log.userID = journal.preparorID;
                log.fromPage = "Affected Journal: " + journal.journalID;
                log.toPage = journal.journalID + " Approved";
                log.journalID = journal.journalID;
                log.userID2 = (int)Session["userID"];
                db.eventLogs.Add(log);
                db.SaveChanges();
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RejectJournal(int journalID, string reason)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (db)
            {
                var account = db.journalTables.Where(x => x.journalID == journalID).FirstOrDefault();
                var ledger = db.ledgerTables.Where(a => a.journalID == journalID).ToList();
                if (account.journalStatus == 0)
                {
                    account.journalStatus = 2;
                    //ledger.lStatus = 0;
                    if (reason != null)
                    {
                       
                        account.reason = reason;

                    }
                    foreach (var v in ledger)
                    {
                        v.lStatus = 0;
                    }

                    account.approverID = (int)Session["userID"];
                }
                else
                {
                    account.journalStatus = 0;
                    foreach (var v in ledger)
                    {
                        v.lStatus = 0;
                    }
                }

                /*eventLog eventLog = new eventLog();
                eventLog.fromPage = "Journal" + journalID + "Approved";
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Account State:" + account.acctStateID;
                eventLog.userID = (int)Session["userID"];
                eventLog.eventDate = DateTime.Now;
                database2.eventLogs.Add(eventLog); */
                account.approverID = (int)Session["userID"];
                eventLog log = new eventLog();
                log.eventDate = DateTime.Now;
                log.userID = account.preparorID;
                log.fromPage = "Affected Journal: " + account.journalID;
                log.toPage = account.journalID + " Rejected";
                log.journalID = account.journalID;
                log.userID2 = (int)Session["userID"];
                db.eventLogs.Add(log);
                db.SaveChanges();
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SuspendJournal(int journalID, string reason)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (db)
            {
                var account = db.journalTables.Where(x => x.journalID == journalID).FirstOrDefault();
                if (account.journalStatus == 0)
                {
                    account.journalStatus = 3;
                    if (reason != null)
                    {
                        account.reason = reason;
                    }
                    account.approverID = (int)Session["userID"];
                }
                else account.journalStatus = 0;

                /*eventLog eventLog = new eventLog();
                eventLog.fromPage = "Journal" + journalID + "Approved";
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Account State:" + account.acctStateID;
                eventLog.userID = (int)Session["userID"];
                eventLog.eventDate = DateTime.Now;
                database2.eventLogs.Add(eventLog); */
                account.approverID = (int)Session["userID"];
                eventLog log = new eventLog();
                log.eventDate = DateTime.Now;
                log.userID = account.preparorID;
                log.fromPage = "Affected Journal: " + account.journalID;
                log.toPage = account.journalID + " Rejected";
                log.journalID = account.journalID;
                log.userID2 = (int)Session["userID"];
                db.eventLogs.Add(log);
                db.SaveChanges();
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ApproveClosingJournal(int journalID)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (db)
            {
                var journal = db.journalTables.Where(x => x.journalID == journalID).FirstOrDefault();
                var ledger = db.ledgerTables.Where(a => a.journalID == journalID).ToList();
                var incomeSummaryExpense = db.accountsTables.Where(a => a.acctNo == 600).SingleOrDefault();
                var retainedEarnings = db.accountsTables.Where(a => a.acctNo == 700).SingleOrDefault();
                if (journal.journalStatus == 0)
                {

                    foreach (var v in ledger)
                    {
                        v.lStatus = 1;
                       
                    }
                    journal.journalStatus = 1;
                    journal.approverID = (int)Session["userID"];
                }
                else
                {
                    journal.journalStatus = 0;
                    foreach (var v in ledger)
                    {
                        v.lStatus = 0;
                    }
                    journal.approverID = (int)Session["userID"];
                }
                db.SaveChanges();
                //incomeSummaryExpense.totalCredits += retainedEarnings.totalCredits;
                //incomeSummaryExpense.balance = (retainedEarnings.balance * -1);
                //incomeSummaryExpense.totalCredits += retainedEarnings.initialBalace;
               // incomeSummaryExpense.initialBalace = incomeSummaryExpense.totalCredits; 
          
                updateBalance(journal.journalID, 2);
                incomeSummaryExpense.initialBalace += incomeSummaryExpense.balance;
                retainedEarnings.totalDebits = 0;
                retainedEarnings.totalCredits = 0;
                retainedEarnings.balance = 0;
                /*eventLog eventLog = new eventLog();
                eventLog.fromPage = "Journal" + journalID + "Approved";
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Account State:" + account.acctStateID;
                eventLog.userID = (int)Session["userID"];
                eventLog.eventDate = DateTime.Now;
                database2.eventLogs.Add(eventLog); */
                eventLog log = new eventLog();
                log.eventDate = DateTime.Now;
                log.userID = journal.preparorID;
                log.fromPage = "Affected Closing Journal: " + journal.journalID;
                log.toPage = journal.journalID + " Approved";
                log.journalID = journal.journalID;
                log.userID2 = (int)Session["userID"];
                db.eventLogs.Add(log);
                db.SaveChanges();
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadClosingData()
        {
            decimal debitAmmount = 0;
            decimal creditAmmount = 0;
            string jID = "";
            string result = "Error! Transaction Is Not Complete!";
            var debits = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
            var credits = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
            // var incomeSummary = db.accountsTables.Where(a => a.acctNo == 0).SingleOrDefault();

            //var acctNoINS = incomeSummary.acctNo;



            int journalID;

            journalTable journal = new journalTable();
            journalID = journal.journalID;
            journal.preparorID = (int)Session["userID"];
            journal.approverID = (int)Session["userID"];
            journal.comments = "Auto-Genrated Closing Entry";
            journal.datePrepared = DateTime.Now;
            journal.isAttach = 0;
            journal.journalStatus = 0;
            journal.reason = "";
            journal.journalTypeID = 3;
            db.journalTables.Add(journal);

            
                foreach (var item in debits)
                {

                    transactionEntryTable entryTable = new transactionEntryTable();

                    entryTable.acctNo = item.acctNo;
                    entryTable.journalID = journal.journalID;
                    //entryTable.transStatusID = 0;
                    entryTable.transactionAmount = item.totalCredits;
                    //
                    debitAmmount += item.totalCredits;
                    //
                    entryTable.transactionType = 2;
                    db.transactionEntryTables.Add(entryTable);
                // entryTable.transStatusID = item.
                //entryTable = transactions;
                db.SaveChanges();
                    eventLog log = new eventLog();
                    log.userID = journal.preparorID;
                    log.transactionID = entryTable.transactionID;
                    log.journalID = journal.journalID;
                    ledgerTable ledgerTable = new ledgerTable();
                    ledgerTable.journalID = journal.journalID;
                    ledgerTable.approvalDate = journal.datePrepared;
                    ledgerTable.acctNo = entryTable.acctNo;
                    ledgerTable.lStatus = 0;
                    ledgerTable.debits = (decimal)entryTable.transactionAmount;
                    log.toPage = "Debited: " + " $" + (decimal)entryTable.transactionAmount;
                    ledgerTable.balance += (decimal)ledgerTable.debits;

                    db.ledgerTables.Add(ledgerTable);
                    log.fromPage = entryTable.acctNo.ToString();
                    log.eventDate = DateTime.Now;
                    db.eventLogs.Add(log);



                }            
      

           foreach (var item in credits)
            {
                transactionEntryTable entryTablec = new transactionEntryTable();

                entryTablec.acctNo = item.acctNo;
                entryTablec.journalID = journal.journalID;
                //entryTable.transStatusID = 0;
                entryTablec.transactionAmount = item.totalDebits;
                creditAmmount += item.totalDebits;
                entryTablec.transactionType = 1;
                db.transactionEntryTables.Add(entryTablec);
                // entryTable.transStatusID = item.
                //entryTable = transactions;

                //
               
                //
                eventLog log = new eventLog();
                log.userID = journal.preparorID;
                log.transactionID = entryTablec.transactionID;
                log.journalID = journal.journalID;
                ledgerTable ledgerTable = new ledgerTable();
                ledgerTable.journalID = journal.journalID;
                ledgerTable.approvalDate = journal.datePrepared;
                ledgerTable.acctNo = entryTablec.acctNo;
                ledgerTable.lStatus = 0;
                ledgerTable.credits = item.totalDebits;
                log.toPage = "Credited: " + " $" + (decimal)entryTablec.transactionAmount;
                ledgerTable.balance += (decimal)ledgerTable.credits;

                db.ledgerTables.Add(ledgerTable);
                log.fromPage = entryTablec.acctNo.ToString();
                log.eventDate = DateTime.Now;
                db.eventLogs.Add(log);

            }
            //db.SaveChanges();
            transactionEntryTable entryTable2 = new transactionEntryTable();

            entryTable2.acctNo = 600;
            entryTable2.journalID = journal.journalID;
            //entryTable.transStatusID = 0;
            entryTable2.transactionAmount = debitAmmount - creditAmmount;
            //incomeSummary.totalCredits = creditAmmount;
            // incomeSummary.totalDebits = debitAmmount;
            entryTable2.transactionType = 1;
            db.transactionEntryTables.Add(entryTable2);
            //db.SaveChanges();
            // entryTable.transStatusID = item.
            //entryTable = transactions;

            //

            //
            eventLog log1 = new eventLog();
            log1.userID = journal.preparorID;
            log1.transactionID = entryTable2.transactionID;
            log1.journalID = journal.journalID;
            ledgerTable ledgerTable1 = new ledgerTable();
            ledgerTable1.journalID = journal.journalID;
            ledgerTable1.approvalDate = journal.datePrepared;
            ledgerTable1.acctNo = entryTable2.acctNo;
            ledgerTable1.lStatus = 0;
            ledgerTable1.credits = (decimal)(decimal)(debitAmmount - creditAmmount);
            log1.toPage = "Credited: " + " $" + (decimal)(debitAmmount-creditAmmount);
            ledgerTable1.balance += (decimal)(decimal)(debitAmmount - creditAmmount);

            db.ledgerTables.Add(ledgerTable1);
            log1.fromPage = entryTable2.acctNo.ToString();
            log1.eventDate = DateTime.Now;
            db.eventLogs.Add(log1);
            db.SaveChanges();
            jID = journal.journalID.ToString();

            //db.SaveChanges();
            result = "Success! Transaction Is Complete!";

            var resultID = Json(new { message = result, journalID = jID });

            return Json(resultID, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Journal()
        {
            List<accountsTable> accountsList = db.accountsTables.Where(x=>x.acctStateID == 1).ToList();
            ViewBag.ListofAccounts = new SelectList(accountsList, "acctNo", "acctName");
            List<journalTypesTable> journalTypesList = db.journalTypesTables.ToList();
            ViewBag.ListofJournalTypes = new SelectList(journalTypesList, "journalTypeID", "journalType");
            return View();

        }

  
        public ActionResult EventLog()
        {
            return View();
        }

        public ActionResult TrialBalance()
        {
            return View();
        }

        public ActionResult IncomeStatement()
        {
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
                            sumTotal -= (i.totalDebits - i.totalCredits) * -1;
                            ///sumTotal = totalDebits;
                        }
                        else
                        {
                            sumTotal -= (i.totalDebits - i.totalCredits) * -1;
                            //sumTotal = totalCredits;
                        }
                    }
                }
                ViewBag.sumTotal = sumTotal.ToString("C", CultureInfo.CurrentCulture);

                if (sumTotal2 == 0)
                {
                    foreach (var i in data2)
                    {

                        if (i.totalDebits > i.totalCredits)
                        {
                            sumTotal2 += (i.totalDebits - i.totalCredits) * -1;
                            ///sumTotal = totalDebits;
                        }
                        else
                        {
                            sumTotal2 += i.totalCredits - i.totalDebits;
                            //sumTotal = totalCredits;
                        }
                    }
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
            return View();
        }

        private void updateBalance(int journalID, int dummy)
        {
            var transations = db.transactionEntryTables.Where(a => a.journalID == journalID).ToList();
            var accounts = db.accountsTables.OrderBy(a => a.acctNo).ToList();

            if (dummy == 1)
            {
                foreach (var a in accounts)
                {
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

                        a.balance = a.initialBalace + a.totalDebits - a.totalCredits;


                    }
                    if (a.totalDebits > a.totalCredits)
                    {
                        a.totalDebits = a.totalDebits - a.totalCredits;
                        a.totalCredits = 0;
                    }
                    else if (a.totalCredits > a.totalDebits)
                    {
                        a.totalCredits = a.totalCredits - a.totalDebits;
                        a.totalDebits = 0;
                    }
                    db.SaveChanges();
                }

            }

            else
            {
                foreach (var a in accounts)
                {
                    foreach (var t in transations)
                    {
                        if (t.acctNo == a.acctNo)
                        {
                            if (t.transactionType == 1)
                            {
                                a.totalDebits -= t.transactionAmount;
                            }
                            else
                            {
                                a.totalCredits -= t.transactionAmount;
                            }
                        }

                        a.balance = a.initialBalace + a.totalCredits - a.totalDebits;
                       
                    }
                    if (a.totalDebits > a.totalCredits)
                    {
                        a.totalDebits = a.totalDebits - a.totalCredits;
                        a.totalCredits = 0;
                    }
                    else if (a.totalCredits > a.totalDebits)
                    {
                        a.totalCredits = a.totalCredits - a.totalDebits;
                        a.totalDebits = 0;
                    }
                    else if(a.totalCredits == a.totalDebits)
                    {
                        a.totalCredits = a.totalCredits - a.totalDebits;
                        a.totalDebits = 0;
                    }
                    
                }
            }
        }

        //shared under other controller with Accountant
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
              db.SaveChanges();
              result = "Success! Transaction Is Complete!";

          }

      }


      return Json(result, JsonRequestBehavior.AllowGet);


  }
  */

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
        */


    }
}