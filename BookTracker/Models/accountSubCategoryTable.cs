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
    
    public partial class accountSubCategoryTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public accountSubCategoryTable()
        {
            this.accountsTables = new HashSet<accountsTable>();
        }
    
        public int acctSubCatID { get; set; }
        public string acctSubCatName { get; set; }
        public int category { get; set; }
    
        public virtual accountCategoryTable accountCategoryTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<accountsTable> accountsTables { get; set; }
    }
}
