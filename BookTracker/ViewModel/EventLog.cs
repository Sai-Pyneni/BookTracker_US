using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookTracker.ViewModels
{
    public class EventLog
    {
       
        public int eventID { get; set; }
        public DateTime eventTimeDate { get; set; }
        public int userID { get; set; }
        public string fromPage { get; set; }
        public string toPage { get; set; }
        public int journalID { get; set; }
        public int transactionID { get; set; }
        public int attachID { get; set; }
        public int ledgerID { get; set; }
        public int userID2{get; set;}

	
    }
}