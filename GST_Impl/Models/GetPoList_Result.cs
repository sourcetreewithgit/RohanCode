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
    
    public partial class GetPoList_Result
    {
        public string PONO { get; set; }
        public string PO_GUID { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<System.DateTime> ExpDeliveryDate { get; set; }
        public Nullable<decimal> TotalAdvance { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StatusDesc { get; set; }
        public string CompanyName { get; set; }
    }
}
