//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GST_Impl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class mst_Statuses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mst_Statuses()
        {
            this.mst_PurchaseOrder = new HashSet<mst_PurchaseOrder>();
        }
    
        public int StatusId { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mst_PurchaseOrder> mst_PurchaseOrder { get; set; }
    }
}
