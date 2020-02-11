//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookTracker.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class journalTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public journalTable()
        {
            this.attachTables = new HashSet<attachTable>();
            this.ledgerTables = new HashSet<ledgerTable>();
            this.transactionEntryTables = new HashSet<transactionEntryTable>();
        }
    
        public int journalID { get; set; }
        public int preparorID { get; set; }
        public string comments { get; set; }
        public int approverID { get; set; }
        public int isAttach { get; set; }
        public Nullable<int> journalTypeID { get; set; }
        public Nullable<System.DateTime> datePrepared { get; set; }
        public Nullable<int> journalStatus { get; set; }
        public string reason { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<attachTable> attachTables { get; set; }
        public virtual userTable userTable { get; set; }
        public virtual journalTypesTable journalTypesTable { get; set; }
        public virtual transactionStatusTable transactionStatusTable { get; set; }
        public virtual userTable userTable1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ledgerTable> ledgerTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transactionEntryTable> transactionEntryTables { get; set; }
    }
}
