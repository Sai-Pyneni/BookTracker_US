using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTracker.Models
{
    [MetadataType(typeof(userTableMetaData))]
    public partial class userTable
    {
        public string confirmPassword { get; set; }
        public List<userRoleTable> userRoleList { get; set; }


    }

    public class userTableMetaData
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name Required")]
        public string userFName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name Required")]
        public string userLName { get; set; }

        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name Required")]
        public string userName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string userEMail { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter Valid Phone Number")]
        public string userPhone { get; set; }

        [Display(Name = "User Role")]
        public int userRoleID { get; set; }

        [Display(Name = "User State")]
        public int userStateID { get; set; }

        [NotMapped]
        public List<userRoleTable> userRoleList { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [MinLength(8)]
        public string userPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("userPassword", ErrorMessage = "Confirm Password and Password Do Not Match")]
        public string confirmPassword { get; set; }






    }

}