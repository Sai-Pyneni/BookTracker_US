using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BookTracker.Models;

namespace BookTracker.Controllers
{

    public class userTableController : Controller
    {
        BT_Entity1 dc = new BT_Entity1();

        // GET: userTable
        [HttpGet]
        public ActionResult Registration()
        {
            /*BTEntity entity = new BTEntity();
            var getUserTypeList = entity.userRoleTables.ToList();
            SelectList list = new SelectList(getUserTypeList, "userRoleID", "userRole");
            ViewBag.userRoleList = list;
            var select = (int)list.SelectedValue;*/
            List<userRoleTable> RoleList = dc.userRoleTables.ToList();
            ViewBag.ListOfRoles = new SelectList(RoleList, "userRoleID", "userRole");

            userTable user = new userTable();
            using (dc)
            {
                user.userRoleList = dc.userRoleTables.ToList<userRoleTable>();
            }

            //  IEnumerable<SelectList> items = dc.userRoleTables.Select(c => new SelectListItem { Value = c.userRole.ToString(), Text = c.userRole });


            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(userTable user)
        {
            //
            string message = "";
            if (ModelState.IsValid)
            {


                using (BT_Entity1 db = new BT_Entity1())
                {
                    // user.userRole = new SelectList(dc.userRoleTables, "userRoleID", "userRole");

                    try
                    {

                        var isExist = IsEmailExist(user.userEMail);

                        var userExist = IsUserExit(user.userName);

                        if (isExist || userExist)
                        {
                            if (isExist && userExist)
                            {
                                ModelState.AddModelError("EmailExist", "Email already exists");
                                ModelState.AddModelError("UserExist", "User already exists");
                            }
                            else if (isExist)
                            {
                                ModelState.AddModelError("EmailExist", "Email already exists");
                            }
                            else
                                ModelState.AddModelError("UserExist", "User already exists");

                            return View(user);
                        }




                        user.userStateID = 0;
                        //user.userID = 1;

                        user.dateCreated = DateTime.Today;
                        //user.lastUpdated = Date
                        if (Session["userID"] != null)
                        {
                            user.createdBy = (int)Session["userID"];
                        }
                        user.userPassword = Crypto.Hash(user.userPassword);
                        user.confirmPassword = Crypto.Hash(user.confirmPassword);
                        db.userTables.Add(user);
                        //dc.userTables.Add(user);

                        eventLog eventLog = new eventLog();
                        eventLog.fromPage = "Registration";
                        eventLog.toPage = "Registration";
                        eventLog.userID = user.userID;
                        eventLog.attachID = 0;
                        eventLog.journalID = 0;
                        eventLog.eventDate = DateTime.Now;
                        eventLog.transactionID = 0;
                        eventLog.ledgerID = 0;
                        eventLog.userID2 = user.userID;





                        db.eventLogs.Add(eventLog);
                        db.SaveChanges();


                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);

                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }



                }
            }
            else
            {
                message = "Invalid Request";
            }
            ViewBag.Message = message;
            if (Session["userID"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "userTable");

            }


        }
        [NonAction]
        public bool IsEmailExist(string email)
        {
            using (dc)
            {
                var v = dc.userTables.Where(a => a.userEMail == email).FirstOrDefault();
                return v != null;
            }
        }

        public bool IsUserExit(string email)
        {
            BT_Entity1 db = new BT_Entity1();
            using (dc)
            {
                var v = db.userTables.Where(a => a.userName == email).FirstOrDefault();
                return v != null;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin userLogin)
        {
            string msg = "";
            userLogin.LoginError = "";
            userLogin.LogOnError = "";
            using (dc)
            {
                var v = dc.userTables.Where(a => a.userName == userLogin.userName).FirstOrDefault();
                eventLog eventLog = new eventLog();
                if (userLogin.userName == null || userLogin.userPassword == null)
                {
                    userLogin.LoginError = "Enter Credentials";
                }
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(userLogin.userPassword), v.userPassword) == 0)
                    {
                        if (v == null)
                        {
                            userLogin.LoginError = "Please Enter your credentials";
                            userLogin.LogOnError = "Please Check your credentials";
                            eventLog.fromPage = "User: " + userLogin.userName + " Attempted Log In, at " + DateTime.Now;
                            eventLog.toPage = "User: " + userLogin.userName + " Entered did not enter username/password. Login Failed";
                            eventLog.eventDate = DateTime.Now;
                            eventLog.userID = v.userID;
                            dc.eventLogs.Add(eventLog);
                            dc.SaveChanges();
                            /// return View("Index", userLogin);
                        }
                        else if (v.userStateID == 1)
                        {

                            if (v.userRoleID == 1 || v.userRoleID == 4)
                            {
                                Session["userID"] = v.userID;
                                Session["userName"] = v.userName;
                                Session["userFName"] = v.userFName;
                                eventLog.fromPage = "User: " + v.userName + "Attempted Log In";
                                eventLog.toPage = "User: " + v.userName + "Signed in as an Admin";
                                eventLog.eventDate = DateTime.Now;
                                eventLog.userID = v.userID;
                                eventLog.userID2 = 0;

                                dc.eventLogs.Add(eventLog);
                                dc.SaveChanges();
                                //ViewBag.Layout = "_Layout.cshtml";
                                View();
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else if (v.userRoleID == 3)
                            {
                                Session["userID"] = v.userID;
                                Session["userName"] = v.userName;
                                Session["userFName"] = v.userFName;
                                eventLog.fromPage = "User: " + v.userName + " Attempted Log In";
                                eventLog.toPage = "User: " + v.userName + " Signed in as an Accountant";
                                eventLog.eventDate = DateTime.Now;
                                eventLog.userID = v.userID;
                                eventLog.userID2 = 0;
                                dc.eventLogs.Add(eventLog);
                                dc.SaveChanges();
                                View();
                                return RedirectToAction("Index", "Accountant");
                            }
                            else if (v.userRoleID == 2)
                            {
                                Session["userID"] = v.userID;
                                Session["userName"] = v.userName;
                                Session["userFName"] = v.userFName;
                                eventLog.fromPage = "User: " + v.userName + " Attempted Log In";
                                eventLog.toPage = "User: " + v.userName + " Signed in as an Manager";
                                eventLog.eventDate = DateTime.Now;
                                eventLog.userID = v.userID;
                                eventLog.userID2 = 0;

                                dc.eventLogs.Add(eventLog);
                                dc.SaveChanges();
                                View();
                                return RedirectToAction("Index", "Manager");
                            }
                            else
                            {
                                eventLog.fromPage = "Unauthorized User Log In" + v.userName + "Attempted Log In, at. Log in Failed";
                                //eventLog.toPage = "User" + v.userName + "Signed in as an Accountant";
                                userLogin.LogOnError = "Anauthorized user- Contact Admin";
                                eventLog.eventDate = DateTime.Now;
                                eventLog.userID = v.userID;
                                eventLog.userID2 = 0;
                                userLogin.LoginError = "Invalid Credentials";
                                dc.eventLogs.Add(eventLog);
                                dc.SaveChanges();
                                //return RedirectToAction("ForgotPassword", "Login");

                            }
                        }

                        else
                        {

                            userLogin.LoginError = "Unauthorized User-Contact Admin";

                        }
                    }
                    else
                    {

                        userLogin.LoginError = "Inavlid Credentials";

                    }


                }
                else if (userLogin.userPassword == null || userLogin.userName == null)
                {

                    userLogin.LogOnError = "Invalid Credentials";

                }
                else
                {
                    userLogin.LoginError = "Invalid Credentials";
                    return RedirectToAction("Login", "userTable");
                }
            }
            ViewBag.Message = msg;

            return View(userLogin);
        }



        public ActionResult LogOut()
        {

            Session.Abandon();
            return RedirectToAction("Login", "userTable");
        }

        public ActionResult ForgotPassword()
        {

            return View("ForgotPassword");
        }


        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify email
            //Generate Reset password link
            //send email 
            string message = "";
            //bool status = false;

            using (dc)
            {
                var account = dc.userTables.Where(a => a.userEMail == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendLink(account.userEMail, resetCode, "ResetPassword");
                    account.resetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    //message = "Reset Link sent!";
                    dc.SaveChanges();
                    message = "Reset Link sent! Check your spam if you do not see email!";

                }

                else
                {
                    message = "Account not found! Contact admin";
                }
            }
            ViewBag.Message = message;
            return View();

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
                subject = "Account Activated";
                body = "Hi<br/><br/> We got your to activate your" +
                    " account. Please visit click link below to login in " +
                    " <br/><br/> <a > " + link + "</a>";
            }
            else if (emailFor == "AccountDeActivated")
            {
                //link = "http://spyneni-001-site1.ctempurl.com";
                subject = "Account DeActivated";
                body = "Hi<br/><br/> Your account has been deactivated" +
                    " account";
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

        public ActionResult ResetPassword(string id)
        {
            //verify reset password
            //find associated with link 
            //redirect to reset password
            using (dc)
            {
                var user = dc.userTables.Where(a => a.resetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.resetCode = id;
                    return View(model);
                }
                else
                    return HttpNotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (dc)
                {
                    var user = dc.userTables.Where(a => a.resetPasswordCode == model.resetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.userPassword = Crypto.Hash(model.newPassword);
                        user.confirmPassword = Crypto.Hash(model.confirmPassword);
                        user.resetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "Password Successfully Updated";
                    }
                }
            }
            else
            {
                message = "Something invalid!-Contact Admin!";
            }
            ViewBag.Message = message;
            return View(model);
        }


    }

}
