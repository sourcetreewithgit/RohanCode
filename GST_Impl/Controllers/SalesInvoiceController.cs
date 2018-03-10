using GST_Impl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Impl.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private GSTBookEntities db = new GSTBookEntities();

        // GET: SalesInvoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string Id)
        {
            TempData["GUID"] = Id;
            return View();
        }

        

        [HttpPost]
        public string Check()
        {
            var GUID = TempData["GUID"];

            dynamic customers = null;
            if (GUID!=null)
            {
                var custId = db.mst_SalesInvoice.Where(g => g.GUID == GUID).Select(s => s.CustomerId).FirstOrDefault();
                customers = db.Customers.Where(c => c.CustomerId == custId).Select(s => new { s.CompanyName, s.CustomerId }).ToList();
            }
            string[] detailArr = new string[1];
            var details = (from d in detailArr
                           select new
                           {
                               GUID= GUID,
                               customers= customers

                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
                    new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();

        }


        [HttpPost]
        public string GetSIList()
        {
            var SI_List = db.mst_SalesInvoice.Select(s => new
            {
                s.GUID,
                s.mst_PurchaseOrder.PONO,
                s.InvoiceCode,
                s.Customer.CompanyName,
                s.GrandTotal,
                s.InvoiceDate,
                s.ReceiptId
            }).ToList();
            return (Newtonsoft.Json.JsonConvert.SerializeObject(SI_List, Newtonsoft.Json.Formatting.None,
            new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();

        }

        [HttpPost]
        public string GetPoList(int CustomerId)
        {
            var Polist = db.mst_PurchaseOrder.Where(c=>c.CustomerId== CustomerId).Select(s => new { s.PO_GUID, s.PONO,s.PO_ID }).ToList();
            return (Newtonsoft.Json.JsonConvert.SerializeObject(Polist, Newtonsoft.Json.Formatting.None,
            new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();

        }

        [HttpPost]
        public string GetPO_InviceDetails(string _param)
        {
            string alert = "";
            var oldInv = db.mst_SalesInvoice.Where(p => p.PoId == db.mst_PurchaseOrder.Where(g => g.PO_GUID == _param).Select(s => s.PO_ID).FirstOrDefault()).Select(i =>new { i.InvoiceId,i.ReceiptId,i.InvoiceCode }).FirstOrDefault();
            
            dynamic SalesInvoice="";
            if (oldInv == null)
            {
                SalesInvoice = db.mst_PurchaseOrder.Where(g => g.PO_GUID == _param).Select(s => new
                {
                    PoId = s.PO_ID,
                    SelectedPoId = _param,
                    CrudStatus = "I", 
                    s.CustomerId,
                    s.GSTN,
                    s.PO_GUID,
                    s.PONO,
                    s.PODate,
                    BilledTo = s.DeliveryAddress,
                    SalesInvoiceItems = s.PurchaseOrderDetails.Select(item => new
                    {
                        item.PoDetailId,
                        item.ProjectCode,
                        item.ItemCode,
                        item.ItemDescription,
                        item.WBS,
                        item.BasicRate,
                        item.Quantity,
                        UnitAdvance = item.ItemAdvance == null ? 0 : (item.ItemAdvance / item.Quantity), 
                        PO_Qty = item.Quantity, 
                        item.ItemAdvance,
                        item.BasicAmt,
                        item.IGSTorCGST_Amt,
                        item.SGSTorUGST_Amt,
                        item.OtherTax_Amt,
                        unitIGSTorCGST_Amt = item.IGSTorCGST_Amt == null ? 0 : (item.IGSTorCGST_Amt / item.Quantity),
                        unitSGSTorUGST_Amt = item.SGSTorUGST_Amt == null ? 0 : (item.SGSTorUGST_Amt / item.Quantity),
                        unitOtherTax_Amt = item.OtherTax_Amt == null ? 0 : (item.OtherTax_Amt / item.Quantity),
                        CrudStatus = "I",
                        Check = false
                    })
                }).FirstOrDefault();
            }
           else if (oldInv != null && oldInv.ReceiptId == null)
            {
                alert = "You can edit the sales invoice - " + oldInv.InvoiceCode + ", already created against this PO from list of invoices.";
            }
            else
            { 
                SalesInvoice = db.mst_PurchaseOrder.Where(g => g.PO_GUID == _param).Select(s => new
                {
                    PoId = s.PO_ID,
                    SelectedPoId = _param,
                    CrudStatus = "I",
                    s.CustomerId,
                    s.PO_GUID,
                    s.PONO,
                    s.PODate,
                    BilledTo = s.DeliveryAddress,
                    SalesInvoiceItems = s.PurchaseOrderDetails.Where(i=>(i.Quantity - 
                    db.ReceiptDetails.Where(r => r.PoDetailId == i.PoDetailId).Sum(q => q.Quantity)>0)).Select(item => new
                    {
                        item.PoDetailId,
                        item.ProjectCode,
                        item.ItemCode,
                        item.ItemDescription,
                        item.WBS,
                        item.BasicRate,
                        item.Quantity,
                        UnitAdvance = item.ItemAdvance == null ? 0 : (item.ItemAdvance / item.Quantity),
                        PO_Qty = item.Quantity- db.ReceiptDetails.Where(r=>r.PoDetailId==item.PoDetailId).Sum(q=>q.Quantity),
                        item.ItemAdvance,
                        item.BasicAmt,
                        item.IGSTorCGST_Amt,
                        item.SGSTorUGST_Amt,
                        item.OtherTax_Amt,
                        unitIGSTorCGST_Amt = item.IGSTorCGST_Amt == null ? 0 : (item.IGSTorCGST_Amt / item.Quantity),
                        unitSGSTorUGST_Amt = item.SGSTorUGST_Amt == null ? 0 : (item.SGSTorUGST_Amt / item.Quantity),
                        unitOtherTax_Amt = item.OtherTax_Amt == null ? 0 : (item.OtherTax_Amt / item.Quantity),
                        CrudStatus = "I",
                        Check = false
                    })
                }).FirstOrDefault();

            }
            string[] detailArr = new string[1];
            var details = (from d in detailArr
                           select new
                           {
                               SalesInvoice = SalesInvoice,
                               alert= alert
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
                    new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();

        }

        [HttpPost]
        public string Edit(string _param)
        {
            var SalesInvoice = db.mst_SalesInvoice.Where(g => g.GUID == _param).Select(s => new
            {
                s.InvoiceId,
                s.mst_PurchaseOrder.PONO,
                s.mst_PurchaseOrder.PODate,
                SelectedPoId = s.mst_PurchaseOrder.PO_GUID,
                s.Acc,
                s.GUID,
                s.InvoiceCode,
                s.InvoiceDate,
                s.CustomerId,
                s.PoId,
                s.BilledTo,
                s.ShippedTo,
                s.Client_Gstn,
                s.Client_PAN_No,
                s.ChallanCode,
                s.ChallanDate,
                s.VendorCode,
                s.E_Way_Bill_No,
                s.PlaceOfSupply,
                s.ReverseCharge,
                s.InsurancePolicy,
                s.PolicyNo,
                s.PolicyStartDt,
                s.PilicyEndDt,
                s.Transport,
                s.LR_No,
                s.LR_Dt,
                s.VehicleNo,
                s.IR_No,
                s.IR_Dt,
                s.GrandTotal,
                s.CreatedBy,
                s.CreatedDt,
                CrudStatus = "U",
                SalesInvoiceItems = s.SalesInvoiceItems.Select(item => new
                {
                    item.PoDetailId,
                    item.PurchaseOrderDetail.ProjectCode,
                    item.PurchaseOrderDetail.ItemCode,
                    item.PurchaseOrderDetail.ItemDescription,
                    item.PurchaseOrderDetail.WBS,
                    item.BasicRate,
                    item.Quantity,
                    UnitAdvance = item.PurchaseOrderDetail.ItemAdvance == null ? 0 : (item.PurchaseOrderDetail.ItemAdvance / item.PurchaseOrderDetail.Quantity),
                    PO_Qty = item.PurchaseOrderDetail.Quantity,
                    item.ItemAdvance,
                    item.BasicAmt,
                    item.IGSTorCGST_Amt,
                    item.SGSTorUGST_Amt,                  
                    item.OtherTax_Amt,
                    unitIGSTorCGST_Amt = item.IGSTorCGST_Amt == null ? 0 : (item.IGSTorCGST_Amt / item.PurchaseOrderDetail.Quantity),
                    unitSGSTorUGST_Amt = item.SGSTorUGST_Amt == null ? 0 : (item.SGSTorUGST_Amt / item.PurchaseOrderDetail.Quantity),
                    unitOtherTax_Amt = item.OtherTax_Amt == null ? 0 : (item.OtherTax_Amt / item.PurchaseOrderDetail.Quantity),
                    Check =true,
                    CrudStatus = "U"
                })
            }).FirstOrDefault();

            var PoList = db.mst_PurchaseOrder.Where(p => p.PO_ID == SalesInvoice.PoId).Select(s => new { s.PO_ID, s.PONO, s.PO_GUID }).ToList();
            var  billedItems= SalesInvoice.SalesInvoiceItems.Select(s => s.PoDetailId).ToList();

            var remainingItems = db.PurchaseOrderDetails.Where(p => p.PO_ID == SalesInvoice.PoId && !billedItems.Contains(p.PoDetailId)).Select(item => new
            {
                item.PoDetailId,
                item.ItemCode,
                item.ProjectCode,
                item.ItemDescription,
                item.WBS,
                item.BasicRate,
                item.Quantity,
                PO_Qty = item.Quantity,
                UnitAdvance = item.ItemAdvance == null ? 0 : (item.ItemAdvance / item.Quantity),
                item.ItemAdvance, 
                item.BasicAmt,
                item.IGSTorCGST_Amt,
                item.SGSTorUGST_Amt,
                item.OtherTax_Amt,
                unitIGSTorCGST_Amt = item.IGSTorCGST_Amt == null ? 0 : (item.IGSTorCGST_Amt / item.Quantity),
                unitSGSTorUGST_Amt = item.SGSTorUGST_Amt == null ? 0 : (item.SGSTorUGST_Amt / item.Quantity),
                unitOtherTax_Amt = item.OtherTax_Amt == null ? 0 : (item.OtherTax_Amt / item.Quantity),
                CrudStatus = "I",
                Check = false
            }).ToList();
        

             string[] detailArr = new string[1];
            var details = (from d in detailArr
                           select new
                           {
                               SalesInvoice = SalesInvoice,
                               remainingItems= remainingItems,
                               PoList= PoList
                           }).FirstOrDefault();
            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
                          new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();

        }



        public string Save(mst_SalesInvoice _param)
        {
            var PO = db.mst_PurchaseOrder.Where(p => p.PO_ID == _param.PoId).FirstOrDefault(); 
            if (_param.CrudStatus=="I")
            {
                _param.CreatedDt = DateTime.Now;
                _param.GUID= Guid.NewGuid().ToString();
              //_param.CreatedBy =CurrentUser.UserId;
                db.mst_SalesInvoice.Add(_param);
                if (PO.StatusId == null)
                {
                    PO.StatusId = 3;
                }  
            }
            else
            {
                var SI = db.mst_SalesInvoice.Where(s => s.InvoiceId == _param.InvoiceId).FirstOrDefault();
                _param.UpdatedDt = DateTime.Now;
                //  _param.UpdatedBy =CurrentUser.UserId; 
                db.Entry<mst_SalesInvoice>(SI).CurrentValues.SetValues(_param);
                if (SI.SalesInvoiceItems!=null)
                {
                    foreach (var item in SI.SalesInvoiceItems)
                    {
                        if (item.CrudStatus=="I")
                        {
                            db.SalesInvoiceItems.Add(item);
                        }
                        else
                        {
                            var siItem = db.SalesInvoiceItems.Where(i => i.Id == item.Id).FirstOrDefault();
                            db.Entry<SalesInvoiceItem>(siItem).CurrentValues.SetValues(item);
                        }
                    
                    }
                }
                if (SI.DeletedSIDetailsList != null)
                {
                    foreach (var item in SI.DeletedSIDetailsList)
                    {
                        var Si_Item = db.SalesInvoiceItems.Where(c => c.Id == item).FirstOrDefault();
                        db.SalesInvoiceItems.Remove(Si_Item);
                    }
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            { 
                throw ex;
            } 
             
            return "S"; 
        } 
        public string DeleteInvoice(string _param)
        {
            var SI = db.mst_SalesInvoice.Where(s => s.GUID == _param).FirstOrDefault();
            if (SI!=null)
            {
                SI.IsDeleted = true;
                db.Entry<mst_SalesInvoice>(SI).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                return "E";
            }
            return "S";

        } 
    }
}