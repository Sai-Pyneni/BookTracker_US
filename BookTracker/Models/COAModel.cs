using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTracker.Models
{
    public class COAModel
    {
        public int acctNo { get; set; }
        public string acctName { get; set; }
        public int acctCat { get; set; }
        public int acctSubCat { get; set; }    
        public decimal balance { get; set; }
       // public decimal initialBalace { get; set; }
        public Nullable<int> acctTerm { get; set; }      
        public string comments { get; set; }
        public Nullable<int> acctStateID { get; set; }

        public string catName { get; set; }
        public string subCatName { get; set; }
        public string state { get; set; }
        public string term { get; set; }
    }
}