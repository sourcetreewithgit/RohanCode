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
    
    public partial class ReceiptDetail
    {
        public int ReceiptDetailId { get; set; }
        public Nullable<int> ReceiptId { get; set; }
        public Nullable<int> SI_Detail_Id { get; set; }
        public Nullable<decimal> BasicRate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> BasicAmt { get; set; }
        public Nullable<int> IGSTorCGST { get; set; }
        public Nullable<int> SGSTorUGST { get; set; }
        public Nullable<int> OtherTax { get; set; }
        public Nullable<decimal> IGSTorCGST_Amt { get; set; }
        public Nullable<decimal> SGSTorUGST_Amt { get; set; }
        public Nullable<decimal> OtherTax_Amt { get; set; }
        public Nullable<decimal> Advance { get; set; }
        public Nullable<decimal> ItemTotal { get; set; }
        public Nullable<int> PoDetailId { get; set; }
    
        public virtual SalesInvoiceItem SalesInvoiceItem { get; set; }
    }
}
