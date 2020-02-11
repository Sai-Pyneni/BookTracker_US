using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTracker.ViewModels
{
    public class UserModel
    {
        public int userID { get; set; }
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
        public int editedBy { get; set; }
    }
}