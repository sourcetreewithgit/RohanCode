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
    
    public partial class CustomerContact
    {
        public int ContactId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
