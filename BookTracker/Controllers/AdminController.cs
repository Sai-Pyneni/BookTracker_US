using BookTracker.Models;
using BookTracker.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BookTracker.Controllers;
using System.Net.Mail;
using System.Net;
using System.Data.Entity.Migrations;

namespace BookTracker.Controllers
{
    public class AdminController : Controller
    {
        private BT_Entity1 database = new BT_Entity1();
        /* -------------------------------------------------------Admin Page Set up----------------------------------------------------------------------*/

        // GET: Admin
        public ActionResult Index()
        {

            /*List<UserModel> list = new List<UserModel>();

            var v = (from a in database.userTables
                     join b in database.userRoleTables on a.userRoleID equals b.userRoleID
                     join c in database.userStateTables on a.userStateID equals c.userStateID
                     select new UserModel
                     {
                         userID = a.userID,
                         userName = a.userName,
                         userFName = a.userFName,
                         userLName = a.userLName,
                         userRole = b.userRole,
                         userState = c.userState,
                         userEMail = a.userEMail,
                         userPhone = a.userPhone

                     });
            list = v.ToList();*/

            
                    

            List<userStateTable> stateList = database.userStateTables.ToList();
            ViewBag.ListOfState = new SelectList(stateList, "userStateID", "userState");
            List<userRoleTable> RoleList = database.userRoleTables.ToList();
            ViewBag.ListOfRoles = new SelectList(RoleList, "userRoleID", "userRole");
            

            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            using (database)
            {
                var data = database.userTables.OrderBy(a => a.userID).ToList();

                //List<accountsTable> accounts =
                var col = data.Select(x => new {
                    x.userID,
                    x.userName,
                    x.userFName,
                    x.userLName,
                    x.userEMail,
                    x.userPhone,
                    x.createdBy,
                    x.dateCreated,
                    x.userRoleTable.userRole,
                    x.userStateTable.userState,                   
                  
                }).ToList();

                return Json(new { data = col }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Edit( int id)
        {
            using (database)
            {
                return View(database.userTables.Where(x => x.userID == id).FirstOrDefault());
            }
           
        }

        [HttpPost]
        public ActionResult Edit(int id, userTable model)
        {
           /*  public int userID { get; set; }
        public string userName { get; set; }
        public string userRole { get; set; }
        public int userStateID { get; set; }
        public string userState { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime lastUpdated { get; set; }
        public string userFName { get; set; }
        public string userLName { get; set; }
        public string userPhone { get; set; }
        public string userEMail { get; set; }
        public string userPassword { get; set; }
        public string resetPasswrodCode { get; set; }
        public int createdBy { get; set; }
        public int editedBy { get; set; }*/

            //var currentAccount = database2.accountsTables.Where(p => p.acctNo = model.acctNo)F;
            // userTable u = new userTable();
            //u.userID = user.userID;
            using (database)
            {
                var user = database.userTables.Where(a => a.userID == id).SingleOrDefault();
                
                user.userID = user.userID;
                user.userName = model.userName;
                user.userLName = model.userLName;
                user.userFName = model.userFName;
                user.userEMail = model.userEMail;
                user.userRoleID = model.userRoleID;
                user.userStateID = model.userStateID;
                user.userPhone = model.userPhone;
                user.userPassword = user.userPassword;
                user.editedBy = (int)Session["userID"];
                user.createdBy = user.createdBy;
                user.confirmPassword = user.userPassword;
                user.resetPasswordCode = user.resetPasswordCode;

              
                //database.Set<userTable>().AddOrUpdate(model);
                database.SaveChanges();

                eventLog eventLog = new eventLog();
                eventLog.fromPage = "Updated User Account:" + user.userName;
                eventLog.userID = (int)Session["userID"];
                eventLog.toPage = "Upated By:" + user.editedBy;
                eventLog.userID = (int)Session["userID"];
                eventLog.userID2 = (int)user.userID;
                eventLog.eventDate = DateTime.Now;

                database.eventLogs.Add(eventLog);
                database.SaveChanges();
                return RedirectToAction("Index");

            }
            // TODO: Add update logic here




        }

        [HttpPost]
        public JsonResult DeactivateByUserID(int userID)
        {
            bool result = false;
            //accountsTable accounts = database2.accountsTables.SingleOrDefault(x => x.acctNo == acctNo );
            using (database)
            {
                var account = database.userTables.Where(x => x.userID == userID).FirstOrDefault();
                try
                {
                    if (account.userStateID == 0)
                    {
                        account.userStateID = 1;
                        SendLink(account.userEMail, "", "AccountActivated".ToString());
                    }
                    else
                    {
                        account.userStateID = 0;
                        SendLink(account.userEMail, "", "AccountDeActivated".ToString());
                    }                      
                    account.userPassword = account.userPassword;
                    account.confirmPassword = account.userPassword;
                    database.SaveChanges();

                    eventLog eventLog = new eventLog();
                    if(account.userStateID == 0)
                    {
                        eventLog.fromPage = "Deactivated user" + userID;
                    }
                    else
                    {
                        eventLog.fromPage = "Activated User" + userID;
                    }
                    
                    eventLog.userID = (int)Session["userID"];
                    eventLog.toPage = "Account State:" + account.userStateID;
                    eventLog.userID = (int)Session["userID"];
                    eventLog.userID2 = (int)account.userID;
                    eventLog.eventDate = DateTime.Now;
                  
                    database.eventLogs.Add(eventLog);
                    database.SaveChanges();
                    result = true;
                }
                catch(Exception ex)
                {
                    result = false;
                }
               

            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [NonAction]
        public void SendLink(string EmailID, string resetCode, string emailFor = "VerifyAccount")
        {
            var verifylink = "/userTable/" + emailFor + "/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifylink);
            link = link.Replace(" ", String.Empty); //take spaces that may be inside, this may help with url issue (DAR)
            //var link = Request.Url.AbsolutePath.Replace(Request.Url.PathAndQuery, verifylink);

            //var fromEmail = new MailAddress("postmaster@booktracker.com", "Book Tracker");
            var toEmail = new MailAddress(EmailID);
            //var fromEmailPassword = "!";

            string subject = "";
            string body = "";

            if (emailFor == "VerifyAccount")
            {

                subject = "Your account is successfully created!";

                body = "<br/> <br/> Here is your account password reset link! <a href= '" + link + "'> " + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi<br/><br/> We got your request to reset your" +
                    " account password. Please click below button to reset or copy the link into your broswer " +
                    " <br/><br/> <a href= '" +
                    link + "'> " + link + "</a>";

            }
            else if (emailFor == "AccountActivated")
            {
                link = "http://spyneni-001-site1.ctempurl.com";
                link = link.Replace(" ", String.Empty);
                subject = "Account Activated";
                body = " <img class='style='max - height: 70px; max - width: 70px;' src='~/ Images / BookTracker_logo.gif' title='BookTracker - logo'" +
                    "Hello,<br/><br/> Your account has been Acctivated!" +
                    ". Please click or copy link below to login in " +
                    " <br/><br/> <a class='btn btn-success'> " + link + "</a>";
            }
            else if (emailFor == "AccountDeActivated")
            {
                //link = "http://spyneni-001-site1.ctempurl.com";
                subject = "Account DeActivated";
                body = " <img class='style='max - height: 70px; max - width: 70px;' src='~/ Images / BookTracker_logo.gif' title='BookTracker - logo'" +
                    "Hi<br/><br/> Your account has been deactivated" +
                    " account. Please contact the admin for access!";
            }
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("booktrackeradm@gmail.com", "btadmin123")
            };
            using (var message = new MailMessage("booktrackeradm@gmail.com", toEmail.Address)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

        }

        public ActionResult Dashboard()
        {
            return View();
        }
        public JsonResult TotalUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userID > 0).ToList();
                int total = 0;


                total = data.Count();

                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ActiveUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userStateID == 1 ).ToList();
                int total = 0;


                 total = data.Count();
              
                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InActiveUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userStateID == 0).ToList();
                int total = 0;


                total = data.Count();

                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AdminUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userRoleID == 1).ToList();
                int total = 0;


                total = data.Count();

                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult  ManagerUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userRoleID == 2).ToList();
                int total = 0;


                total = data.Count();

                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AccountantUsers()
        {
            using (database)
            {
                var data = database.userTables.Where(a => a.userRoleID == 3).ToList();
                int total = 0;


                total = data.Count();

                //decimal.Round(ratio,2);



                return Json(total, JsonRequestBehavior.AllowGet);
            }
        }
        /*
        /* ------------------------------------------------------- Create User ----------------------------------------------------------------------

        // POST: Administrator/Create
        [HttpPost]
        public ActionResult Create(userTable user)
        {
            return View();
        }

        /*---------------------------------- Update User Values, used for all user update functions ----------------------------------------------------
        [HttpPost]
        public ActionResult saveuser(int id, string propertyName, string value)
        {

            var status = false;
            var message = "";
            var user = database.userTables.Find(id);
            var eventLog = database.eventLogs;


            object updateValue = value;
            bool isValid = true;

            if (propertyName == "userRoleID")
            {
                int newRoleID = 0;
                if (int.TryParse(value, out newRoleID))
                {
                    updateValue = newRoleID;
                    value = database.userRoleTables.Where(a => a.userRoleID == newRoleID).First().userRole;
                }
                else
                {
                    isValid = false;
                }
            }


            if (propertyName == "userStateID")
            {
                int newStateID = 0;
                if (int.TryParse(value, out newStateID))
                {
                    updateValue = newStateID;
                    value = database.userStateTables.Where(a => a.userStateID == newStateID).First().userState;
                }
                else
                {
                    isValid = false;
                }
            }

            if (user != null && isValid)
            {

                database.Entry(user).Property(propertyName).CurrentValue = updateValue;
                database.SaveChanges();
                status = true;

            }

            else
            {
                message = "Error!";
            }


            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());


        }

        public ActionResult GetUserRoles(int id)
        {

            //Allowed response content format
            //{'E':'Letter E','F':'Letter F','G':'Letter G', 'selected':'F'}
            int selectedRoleID = 0;
            StringBuilder sb = new StringBuilder();

            var roles = database.userRoleTables.OrderBy(a => a.userRole).ToList();
            foreach (var item in roles)
            {
                sb.Append(string.Format("'{0}':'{1}',", item.userRoleID, item.userRole));
            }

            selectedRoleID = database.userTables.Where(a => a.userID == id).First().userRoleID;


            sb.Append(string.Format("'selected': '{0}'", selectedRoleID));
            return Content("{" + sb.ToString() + "}");
        }

        public ActionResult GetState(int id)
        {

            //Allowed response content format
            //{'E':'Letter E','F':'Letter F','G':'Letter G', 'selected':'F'}
            int selectedActive = 0;
            StringBuilder sb = new StringBuilder();

            var roles = database.userStateTables.OrderBy(a => a.userState).ToList();
            foreach (var item in roles)
            {
                sb.Append(string.Format("'{0}':'{1}',", item.userStateID, item.userState));
            }

            selectedActive = database.userTables.Where(a => a.userID == id).First().userStateID;


            sb.Append(string.Format("'selected': '{0}'", selectedActive));
            return Content("{" + sb.ToString() + "}");
        } */

    }
}