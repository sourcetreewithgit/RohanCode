using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GST_Impl.Models;

namespace GST_Impl.Controllers
{
    public class POController : Controller
    {
        private GSTBookEntities db = new GSTBookEntities();
        // GET: PO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string Id,string IsAmmendment="N")
        {
            TempData["GUID"] = Id;
            TempData["IsAmmendment"] = IsAmmendment; 
            return View();
        }

        public string Init() 
        {
            var GUID = TempData["GUID"];
            string IsAmmendment = TempData["IsAmmendment"].ToString();

            
            dynamic PO;
            if (GUID == null)
            {
                PO = new mst_PurchaseOrder();
                PO.CrudStatus = "I";
                //var fdhsj = db.GetPOByGuid("sdsff-dg");
            }
            else
            {
                var _GUID = GUID.ToString();
                var Check = db.mst_PurchaseOrder.Where(g => g.PO_GUID == _GUID).Select(s => new { s.StatusId,s.PO_ID }).FirstOrDefault();
                if (IsAmmendment != "N" && Check.StatusId != 2)
                {
                    return "Insecure request";
                } 
               
                PO = (from po in db.mst_PurchaseOrder
                      where po.PO_GUID == _GUID
                      select new
                      {
                          po.PO_ID,
                          po.PO_GUID,
                          po.PONO,
                          po.CustomerId,
                          po.PODate,
                          po.DeliveryAddress,
                          po.PurchaseOffice,
                          po.ExpDeliveryDate,
                          po.ActualDelDate,
                          po.BuyFromBP,
                          po.BuyFromOrder,
                          po.AmendNo,
                          po.AmendDate,
                          po.AmendRemarks,
                          po.StatusId,
                          po.RefA,
                          po.RefB,
                          po.GSTN,
                          po.Buyer,
                          ParentId = IsAmmendment == "N" ? po.ParentId: Check.PO_ID,
                          po.TotalAdvance,
                          po.BalanceAdvance,
                          po.GrandTotal,
                          po.CreatedDt,
                          po.CreatedBy, 
                          CrudStatus = IsAmmendment=="N"?"U":"I",
                          DeletedPODetails = "",
                          PurchaseOrderDetails = po.PurchaseOrderDetails.Select(s => new
                          {
                              s.PoDetailId,
                              s.PO_ID,
                              s.ProjectCode,
                              s.ItemDescription,
                              s.WBS,
                              s.ItemCode,
                              s.Quantity,
                              s.UomId,
                              s.ItemAdvance,
                              s.BasicRate,
                              s.BasicAmt,
                              s.TotalBasic,
                              s.IGSTorCGST,
                              s.SGSTorUGST,
                              s.OtherTax,
                              s.IGSTorCGST_Amt,
                              s.SGSTorUGST_Amt,
                              s.OtherTax_Amt, 
                              s.HsnSacCode,
                              s.ItemDelDate, 
                              s.Total,
                              s.IsDelivered,
                              CrudStatus = IsAmmendment == "N" ? "U" : "I"
                          }),                         
                          po.Customer.CompanyName,
                          po.Customer.CompanyIdNo,
                          po.Customer.LocAddress,
                          po.Customer.Telephone
                      }).FirstOrDefault();

            }

            var TaxList = db.GetTaxList();
            var Uoms = db.GetUoms();
            var Customers = db.Customers.Select(s => new { s.CompanyName, s.CustomerId }).ToList();
            var HsnCodes = db.HsnHacCodes.Select(s => new { s.HsnHacCodeId, s.Code }).ToList();
            string[] detailArr = new string[1];

            var details = (from d in detailArr
                           select new
                           {
                               PO = PO,
                               TaxList = TaxList,
                               Uoms= Uoms,
                               Customers= Customers,
                               IsAmd=IsAmmendment,
                               HsnCodes= HsnCodes
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
                new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore})).ToString();
        }
        

        //Created by and updated by should be included after login service is provided -Anand
        public string Save(mst_PurchaseOrder _paramPO,string status)
        {

            var Parent = db.mst_PurchaseOrder.Where(g => g.PO_ID == _paramPO.ParentId).FirstOrDefault();

            if (Parent != null && Parent.StatusId!=2)
            {
                return "Request cannot be submitted.";
            }
            if (Parent!=null)
            {
                _paramPO.AmendNo = (Parent.AmendNo==null?0:Parent.AmendNo) + 1;
                _paramPO.AmendDate = DateTime.Now;
            } 
            if (_paramPO.CrudStatus == "I")
            {
                _paramPO.PO_GUID = Guid.NewGuid().ToString();
                _paramPO.CreatedDt = DateTime.Now;

                if (status == "S")
                {
                    _paramPO.StatusId = 2; 
                }
                else
                {
                    _paramPO.StatusId = 1;
                }

                db.mst_PurchaseOrder.Add(_paramPO);
            }
            else
            {
                var oldPO = (from po in db.mst_PurchaseOrder where po.PO_GUID == _paramPO.PO_GUID select po).FirstOrDefault();
                _paramPO.UpdatedDt = DateTime.Now;

                if (status=="S")
                {
                    if (oldPO.AmendNo==null|| oldPO.AmendNo ==0)
                    {
                        _paramPO.AmendNo = 1; 
                    }
                    else
                    {
                        _paramPO.AmendNo  = oldPO.AmendNo + 1;
                    }
                    _paramPO.AmendDate = DateTime.Now;
                    _paramPO.StatusId = 2;
                }
                else
                {
                    _paramPO.StatusId = 1;
                }
                
                db.Entry<mst_PurchaseOrder>(oldPO).CurrentValues.SetValues(_paramPO);

                foreach (var item in oldPO.PurchaseOrderDetails)
                {
                    if (item.CrudStatus == "I")
                    {
                        db.PurchaseOrderDetails.Add(item);
                    }
                    else
                    {
                        var oldPODetail = db.PurchaseOrderDetails.Find(item.PoDetailId);
                        db.Entry<PurchaseOrderDetail>(oldPODetail).CurrentValues.SetValues(item);
                    }
                    
                }

                if (_paramPO.DeletedPODetails != null)
                {
                    if (_paramPO.DeletedPODetails.Count > 0)
                    {
                        foreach (var item in _paramPO.DeletedPODetails)
                        {
                            var oldPODetail = db.PurchaseOrderDetails.Find(item.PoDetailId);
                            db.PurchaseOrderDetails.Remove(oldPODetail);
                        }
                    }
                } 
            }
            if (Parent!=null)
            {

          
            Parent.IsAmmended = true;
            db.Entry<mst_PurchaseOrder>(Parent).State = System.Data.Entity.EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception e)
            {
                return "E";
            }
        }

        public string GetPOList()
        {
            var polist = db.GetPoList();
            return (Newtonsoft.Json.JsonConvert.SerializeObject(polist, Newtonsoft.Json.Formatting.None,
               new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }
        public string GetCustomerPOList(int _param)
        {
            var polist = db.mst_PurchaseOrder.Where(c=>c.CustomerId==_param && c.IsAmmended!=true).Select(p=>new {p.PO_GUID,p.PONO });
            return (Newtonsoft.Json.JsonConvert.SerializeObject(polist, Newtonsoft.Json.Formatting.None,
               new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }


    }
}