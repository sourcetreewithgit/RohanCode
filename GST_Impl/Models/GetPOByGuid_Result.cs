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
    
    public partial class GetPOByGuid_Result
    {
        public int PO_ID { get; set; }
        public string PO_GUID { get; set; }
        public string PONO { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public string DeliveryAddress { get; set; }
        public string PurchaseOffice { get; set; }
        public Nullable<System.DateTime> ExpDeliveryDate { get; set; }
        public Nullable<System.DateTime> ActualDelDate { get; set; }
        public string BuyFromBP { get; set; }
        public string BuyFromOrder { get; set; }
        public Nullable<int> AmendNo { get; set; }
        public Nullable<System.DateTime> AmendDate { get; set; }
        public string AmendRemarks { get; set; }
        public string RefA { get; set; }
        public string RefB { get; set; }
        public string GSTN { get; set; }
        public string Buyer { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<decimal> TotalAdvance { get; set; }
        public Nullable<decimal> BalanceAdvance { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<bool> IsAmmended { get; set; }
        public Nullable<System.DateTime> CreatedDt { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int PoDetailId { get; set; }
        public Nullable<int> PO_ID1 { get; set; }
        public string ProjectCode { get; set; }
        public string WBS { get; set; }
        public string ItemCode { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<int> UomId { get; set; }
        public Nullable<decimal> ItemAdvance { get; set; }
        public Nullable<decimal> BasicRate { get; set; }
        public Nullable<decimal> BasicAmt { get; set; }
        public Nullable<decimal> TotalBasic { get; set; }
        public Nullable<int> IGSTorCGST { get; set; }
        public Nullable<int> SGSTorUGST { get; set; }
        public Nullable<int> OtherTax { get; set; }
        public Nullable<decimal> IGSTorCGST_Amt { get; set; }
        public Nullable<decimal> SGSTorUGST_Amt { get; set; }
        public Nullable<decimal> OtherTax_Amt { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<int> HsnSacCode { get; set; }
        public Nullable<System.DateTime> ItemDelDate { get; set; }
        public string ItemDescription { get; set; }
    }
}
