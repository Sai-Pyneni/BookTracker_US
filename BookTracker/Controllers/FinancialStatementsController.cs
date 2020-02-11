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
    public class FinancialStatementsController : Controller
    {
        private BT_Entity1 db = new BT_Entity1();
        #region: Balance Sheet
        public JsonResult CurrentAssets()       {

            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 1 && a.acctTerm == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
               
                foreach(var i in data)
                {
                    if (i.balance < 0)
                    {
                        i.balance = i.balance * -1;
                    }
                   
                }
                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.acctName,
                    x.balance



                });

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult LongTermAssets()
        {

            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 1 && a.acctTerm == 2 &&(a.totalDebits != 0 || a.totalCredits != 0)).ToList();


                foreach (var i in data)
                {
                    if (i.acctNo == 182)
                    {
                        if (i.balance > 0)
                        {
                            i.balance = i.balance * -1;
                        }
                        else if (i.balance < 0)
                        {
                            i.balance = i.balance;
                        }

                    }
                    else if (i.balance < 0)
                    {
                        i.balance = i.balance * -1;
                    }
                    else
                        i.balance = i.balance * 1;
                       
                 
                }
                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.acctName,
                    x.balance



                });

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult TotalAssets()
        {

            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
               // var total = "Total Assets";
                decimal sumTotal = 0;
             
                if(sumTotal == 0)
                {
                    foreach (var i in data)
                    {
                    
                        if (i.totalDebits > i.totalCredits)
                        {
                            sumTotal += i.totalDebits - i.totalCredits;
                            ///sumTotal = totalDebits;
                        }
                        else
                        {
                            sumTotal -= i.totalDebits - i.totalCredits;
                            //sumTotal = totalCredits;
                        }
                    }
                }
                ViewBag.sumTotal = sumTotal;
                var col = sumTotal;

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult OwnersEquity()
        {

            using (db)
            {
                var data = db.accountsTables.Where(a => (a.acctCat == 3 || a.acctCat == 6) && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();


                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                foreach (var i in data)
                {
                    if (i.balance < 0)
                    {
                        i.balance = i.balance * -1;
                    }
                 
                }
                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.acctName,
                    x.balance



                });

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CurrentLiabilites()
        {

            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 2 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();


                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                foreach (var i in data)
                {
                    if (i.balance < 0)
                    {
                        i.balance = i.balance * -1;
                    }
                   
                }
                //data = data.Where(a => a.totalCredits > 0 || a.totalCredits > 0).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new
                {
                    x.acctName,
                    x.balance



                });
                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult TotalLiabilites()
        {

            using (db)
            {
                var data2 = db.accountsTables.Where(a => (a.acctCat == 2 || a.acctCat == 3) && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                decimal sumTotal = 0;
                decimal totalDebits = 0;
                decimal totalCredits = 0;
                foreach (var i in data2)
                {
                    totalDebits += i.totalDebits;
                    totalCredits += i.totalCredits;
                }
                if (totalDebits > totalCredits)
                {
                    totalDebits = totalDebits - totalCredits;
                    sumTotal = totalDebits;
                }
                else if (totalCredits > totalDebits)
                {
                    totalCredits = totalCredits - totalDebits;
                    sumTotal = totalCredits;
                }


                var col = data2.Select(x => new
                {

                    sumTotal


                });
                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }

        }
    
        public JsonResult CurrentRatio()
        {
            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 2 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                var data2 = db.accountsTables.Where(a => a.acctCat == 1 && a.acctTerm == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                decimal currentAssetsTotal = 0;
                decimal currentLialbiltiesTotal = 0;
                decimal ratio = 0;
                foreach (var i in data)
                {
                    currentLialbiltiesTotal += i.totalCredits;
                    currentLialbiltiesTotal -= i.totalDebits;
                  
                }
                foreach (var i in data2)
                {
                    currentAssetsTotal += i.totalDebits;
                    currentAssetsTotal -= i.totalCredits;
                }
                ratio = (currentAssetsTotal / currentLialbiltiesTotal)*100;
                //decimal.Round(ratio,2);


              
                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult QuickRatio()
        {
            using (db)
            {
                var data = db.accountsTables.Where(a => a.acctCat == 2 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                var data2 = db.accountsTables.Where(a => a.acctCat == 1 && a.acctTerm == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();
                decimal currentAssetsTotal = 0;
                decimal currentLialbiltiesTotal = 0;
                decimal ratio = 0;
                foreach (var i in data)
                {
                    currentLialbiltiesTotal += i.totalCredits;
                    currentLialbiltiesTotal -= i.totalDebits;

                }
                foreach (var i in data2)
                {
                    currentAssetsTotal += i.totalDebits;
                    currentAssetsTotal -= i.totalCredits;
                }
                ratio = (currentAssetsTotal / currentLialbiltiesTotal) * 100;
                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult returnOnAssets()
        {
            using (db)
            {
                decimal total = 0;
                decimal ratio = 0;
                decimal assets = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                var data2 = db.accountsTables.Where(a => a.acctCat == 1 && a.balance != 0).ToList();
                var earnings = db.accountsTables.Where(a => a.acctNo == 600).SingleOrDefault();

                foreach (var r in revs)
                {
                    total -= r.balance;
                }
                foreach (var r in exps)
                {
                    total -= r.balance;
                }
                if(total == 0)
                {
                    total = earnings.initialBalace;
                }
                foreach (var r in data2)
                {
                    
                        assets += r.totalDebits;
                        assets -= r.totalCredits;
                   
                }
                
                ratio = (total / assets) * 100;
                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult returnOnEquity()
        {
            using (db)
            {
                decimal total = 0;
                decimal ratio = 0;
                decimal assets = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                var data2 = db.accountsTables.Where(a => a.acctCat == 3 && a.balance != 0).ToList();
                var earnings = db.accountsTables.Where(a => a.acctNo == 600).SingleOrDefault();

                foreach (var r in revs)
                {
                    total -= r.balance;
                }
                foreach (var r in exps)
                {
                    total -= r.balance;
                }
                if (total == 0)
                {
                    total = earnings.initialBalace;
                }
                foreach (var r in data2)
                {

                    assets -= r.totalDebits;
                    assets += r.totalCredits;

                }

                ratio = (total / assets) * 100;
                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult npmRatio()
        {
            using (db)
            {
                decimal revenues = 0;
                decimal ratio = 0;
                decimal expenses = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var exps = db.accountsTables.Where(a => a.acctCat == 5 && a.balance != 0).ToList();
                var data2 = db.accountsTables.Where(a => a.acctCat == 3 && a.balance != 0).ToList();
                var earnings = db.accountsTables.Where(a => a.acctNo == 600).SingleOrDefault();

                foreach (var r in revs)
                {
                    revenues += r.totalCredits;
                }
                foreach (var r in exps)
                {
                    expenses += r.totalDebits;
                }                
               
                if(revenues == 0)
                {
                    ratio = 0;
                }
                else
                {
                    ratio = ((revenues - expenses) / revenues) * 100;
                }
               


                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult faTO()
        {
            using (db)
            {
                decimal revenues = 0;
                decimal ratio = 0;
                decimal assets = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var fiexsAssets = db.accountsTables.Where(a => a.acctCat == 1 && a.acctTerm == 2 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();

                foreach (var r in revs)
                {
                    revenues += r.totalCredits;
                    revenues -= r.totalDebits;
                }
               foreach(var x in fiexsAssets)
                {
                    assets += x.totalDebits;
                    assets -= x.totalCredits;
                }
               
                 

                if(revenues == 0)
                {
                    ratio = 0;
                }
                else
                {
                    ratio = (revenues / assets) * 100;
                }

                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult taTO()
        {
            using (db)
            {
                decimal revenues = 0;
                decimal ratio = 0;
                decimal assets = 0;
                var revs = db.accountsTables.Where(a => a.acctCat == 4 && a.balance != 0).ToList();
                var fiexsAssets = db.accountsTables.Where(a => a.acctCat == 1 && (a.totalDebits != 0 || a.totalCredits != 0)).ToList();

                foreach (var r in revs)
                {
                    revenues += r.totalCredits;
                    revenues -= r.totalDebits;
                }
                foreach (var x in fiexsAssets)
                {
                    assets += x.totalDebits;
                    assets -= x.totalCredits;
                }

                if (revenues == 0)
                {
                    ratio = 0;
                }
                else
                {
                    ratio = (revenues / assets) * 100;
                }


                //decimal.Round(ratio,2);



                return Json(decimal.Round(ratio, 2), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}