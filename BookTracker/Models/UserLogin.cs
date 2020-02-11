using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookTracker.Models
{
    public class UserLogin
    {
        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="User Name Required")]
        public string   userName { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name Required")]
        [DataType(DataType.Password)]
        public string userPassword { get; set; }

        public string LoginError { get; set; }
        public string LogOnError { get; set; }

    }
}