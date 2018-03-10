using GST_Impl.Models;
using System;
using System.Collections.Generic;

namespace GST_Impl.Models
{
    public partial class mst_PurchaseOrder
    {
        public string CrudStatus { get; set; }
    
        public List<PurchaseOrderDetail> DeletedPODetails { get; set; }

    }

    public partial class PurchaseOrderDetail
    {
        public string CrudStatus { get; set; }

    }
    public partial class mst_SalesInvoice
    {
        public string CrudStatus { get; set; } 
        public List<int> DeletedSIDetailsList { get; set; }

    }
    public partial class SalesInvoiceItem
    {
        public string CrudStatus { get; set; }

    }
    public partial class Customer
    {
        public string CrudStatus { get; set; }
        public List<CustomerContact> DeletedCustomerContactDetails { get; set; }

    }

    public partial class CustomerContact
    {
        public string CrudStatus { get; set; }

    }

    public partial class mst_Uom
    {
        public string CrudStatus { get; set; }

    }
    public partial class HsnHacCode
    {
        public string CrudStatus { get; set; }

    }
}
