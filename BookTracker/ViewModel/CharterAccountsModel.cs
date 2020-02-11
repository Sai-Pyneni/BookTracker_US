using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace BookTracker.ViewModels
{
    public class CharterAccountsModel
    {
        [Required(ErrorMessage ="Account Number is Required")]
        public int acctNo { get; set; }

        [Required(ErrorMessage = "Account Name is Required")]
        public string acctName { get; set; }

        [Required(ErrorMessage = "Pick a Category")]
        public int acctCat { get; set; }

        [Required(ErrorMessage = "Pick a Sub-Category")]
        public int acctSubCat { get; set; }
        public decimal totalDebits { get; set; }
        public decimal totalCredits { get; set; }
        public decimal balance { get; set; }
        public decimal initialBalace { get; set; }
        [Required(ErrorMessage = "Pick Account Term")]
        public Nullable<int> acctTerm { get; set; }
        public Nullable<int> createdBy { get; set; }
        public string comments { get; set; }
        public Nullable<int> editedBy { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        [Required(ErrorMessage = "Pick Status")]
        public Nullable<int> acctStateID { get; set; }

        public string catName { get; set; }
        public string acctSubCatName { get; set; }
        public string actState { get; set; }
    }
}