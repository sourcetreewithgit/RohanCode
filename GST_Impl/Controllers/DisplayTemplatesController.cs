using GST_Impl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Impl.Controllers
{
    public class DisplayTemplatesController : Controller
    {
        private GSTBookEntities db = new GSTBookEntities();

        // GET: DisplayTemplates 
        public  ActionResult DisplayPO(string Id)
        {
            TempData["GUID"] = Id; 
            return View();
        }
        [HttpPost]
        public string DisplayPO()
        {
            string Id = TempData["GUID"].ToString(); 
              var  PO = (from po in db.mst_PurchaseOrder
                      where po.PO_GUID == Id
                         select new
                      { 
                          po.PONO,
                          po.Customer.CompanyName,
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
                          po.RefA,
                          po.RefB,
                          po.GSTN,
                          po.Buyer,  
                          po.TotalAdvance,
                          po.BalanceAdvance,
                          po.GrandTotal,
                          po.CreatedDt,
                          po.CreatedBy,
                          po.UpdatedDt,
                          po.UpdatedBy, 
                          PurchaseOrderDetails = po.PurchaseOrderDetails.Select(s => new
                          { 
                              s.ProjectCode,
                              s.WBS,
                              s.ItemCode,
                              s.Quantity,
                              s.UomId,
                              s.ItemAdvance,
                              s.BasicRate,
                              s.BasicAmt,
                              s.TotalBasic,
                              IGSTorCGST = s.mst_Taxes.Name,
                              SGSTorUGST=s.mst_Taxes1.Name,
                              OtherTax=s.mst_Taxes2.Name,
                              s.IGSTorCGST_Amt,
                              s.SGSTorUGST_Amt,
                              s.OtherTax_Amt, 
                              s.Total,
                              s.IsDelivered,
                              s.HsnHacCode.Code,
                              s.ItemDelDate,
                              s.ItemDescription                             
                          }), 
                          po.Customer.CompanyIdNo,
                          po.Customer.LocAddress,
                          po.Customer.Telephone
                      }).FirstOrDefault();

            

            return (Newtonsoft.Json.JsonConvert.SerializeObject(PO, Newtonsoft.Json.Formatting.None,
                new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

    }
}