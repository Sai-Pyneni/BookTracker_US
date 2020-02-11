using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTracker.Models
{
    public class COAEditModel
    {
        public int acctNo { get; set; }
        public string acctName { get; set; }
        public int acctCat { get; set; }
        public int acctSubCat { get; set; }
        public decimal totalDebits { get; set; }
        public decimal totalCredits { get; set; }
        public decimal balance { get; set; }
        public decimal initialBalace { get; set; }
        public Nullable<int> acctTerm { get; set; }
        public Nullable<int> createdBy { get; set; }
        public string comments { get; set; }
        public Nullable<int> editedBy { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public byte[] dateEdited { get; set; }
        public Nullable<int> acctStateID { get; set; }
    }
}