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
    
    public partial class accountCategoryTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public accountCategoryTable()
        {
            this.accountsTables = new HashSet<accountsTable>();
            this.accountSubCategoryTables = new HashSet<accountSubCategoryTable>();
        }
    
        public int acctCatID { get; set; }
        public string catName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<accountsTable> accountsTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<accountSubCategoryTable> accountSubCategoryTables { get; set; }
    }
}