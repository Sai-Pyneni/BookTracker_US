using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTracker.Models
{
    
    public class userDetails
    {
        public List<userTable> list;

        public int userID { get; set; }

        [Display(Name = "First Name")]
      
        public string userFName { get; set; }

        [Display(Name = "Last Name")]
       
        public string userLName { get; set; }

        [Display(Name = "User Name")]
       
        public string userName { get; set; }

        [Display(Name = "Email")]
       
        public string userEMail { get; set; }

        [Display(Name = "Phone")]
       
        public string userPhone { get; set; }

        [Display(Name = "User Role")]
        public int userRole { get; set; }

      
        [Display(Name = "Password")]      
        public string userPassword { get; set; }


        public string userState { get; set; }


    }
  

}