using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BookTracker.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage ="New password required", AllowEmptyStrings =false)]
        [Display(Name ="Enter New Password:")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,36}$", ErrorMessage ="Password must contain:" +
            "\n - Minimum 8 Characters, Maximum 36 characters" +
             "\n - Atleast 1 capital letter and 1 lower case latter" +
            "\n - Atleast one special character" +
            "\n - Atleast one digit")]

        public  string newPassword { get; set; }

        [Display(Name = "Confirm Password:")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage ="New and Confirm Password must match")]
        public string confirmPassword { get; set; }
        [Required]
        public string resetCode { get; set; }
    }
}