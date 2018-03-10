using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GST_Impl.Models;

namespace GST_Impl.Controllers
{
    public class MasterController : Controller
    {
        private GSTBookEntities db = new GSTBookEntities();
        private Common _common;
        public MasterController()
        {
            _common = new Common();
        }
        // GET: Master
        public ActionResult UMOIndex()
        {
            return View();
        }
        public ActionResult GSTIndex()
        {
            return View();
        }

        public ActionResult HSNIndex()
        {
            return View();
        }

        public string UMOInit()
        {
            mst_Uom umo = new mst_Uom();
            umo.CrudStatus = "I";
            var UMOList = _common.GetUMOList().Select(s => new { s.UomId, s.Description, CrudStatus = "U" });

            string[] detailArr = new string[1];

            var details = (from d in detailArr
                           select new
                           {
                               umo = umo,
                               UMOList = UMOList
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
              new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string GetUMOById(int _pUomId)
        {
            var umo = db.mst_Uom.Where(u => u.UomId == _pUomId).Select(s => new { s.UomId, s.Description, CrudStatus = "U" }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(umo, Newtonsoft.Json.Formatting.None,
             new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string UMODelete(int _pUomId)
        {
            var umoOldObj = db.mst_Uom.Find(_pUomId);
            db.mst_Uom.Remove(umoOldObj);
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception)
            {
                return "E";
            }

        }

        public string UMOSave(mst_Uom _pUmo)
        {
            if (_pUmo.CrudStatus == "I")
            {
                //umo.CreatedBy = "";
                _pUmo.CreatedDt = DateTime.Now;
                db.mst_Uom.Add(_pUmo);
            }
            else
            {
                //umo.UpdatedBy = "";
                _pUmo.UpdatedDt = DateTime.Now;

                var umoOldObj = db.mst_Uom.Find(_pUmo.UomId);
                db.Entry(umoOldObj).CurrentValues.SetValues(_pUmo);
            }
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception)
            {
                return "E";
            }
        }

        public string GSTInit()
        {
            mst_Taxes gst = new mst_Taxes();

            //var GSTList = db.GetTaxList();

            var GSTList = (from g in db.mst_Taxes where g.Percentage != 0 select new { g.TaxId,g.Name,g.mst_TaxTypes.Description}).ToList();

            var GSTTypes = _common.GetTaxTypeList().Select(s => new { s.TaxTypeId,s.Description});

            string[] detailArr = new string[1];

            var details = (from d in detailArr
                           select new
                           {
                               gst = gst,
                               GSTList = GSTList,
                               GSTTypes = GSTTypes
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
              new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string GSTSave(mst_Taxes _pgst)
        {
            //gst.CreatedBy = "";
            _pgst.CreatedDt = DateTime.Now;
            db.mst_Taxes.Add(_pgst);
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception)
            {
                return "E";
            }
        }

        public string HSNInit()
        {
            HsnHacCode hsn = new HsnHacCode();
            hsn.CrudStatus = "I";
            var HSNList = _common.GetHSNList().Select(s => new { s.HsnHacCodeId, s.Code });

            string[] detailArr = new string[1];

            var details = (from d in detailArr
                           select new
                           {
                               hsn = hsn,
                               HSNList = HSNList
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
              new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string GetHSNById(int _pHsnHacCodeId)
        {
            var hsn = db.HsnHacCodes.Where(u => u.HsnHacCodeId == _pHsnHacCodeId).Select(s => new { s.HsnHacCodeId, s.Code, CrudStatus = "U" }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(hsn, Newtonsoft.Json.Formatting.None,
             new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string HSNDelete(int _pHsnHacCodeId)
        {
            var hsnOldObj = db.HsnHacCodes.Find(_pHsnHacCodeId);
            db.HsnHacCodes.Remove(hsnOldObj);
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception)
            {
                return "E";
            }

        }

        public string HSNSave(HsnHacCode _phsn)
        {
            if (_phsn.CrudStatus == "I")
            {
                //umo.CreatedBy = "";
                _phsn.CreatedDt = DateTime.Now;
                db.HsnHacCodes.Add(_phsn);
            }
            else
            {
                //umo.UpdatedBy = "";
                _phsn.UpdatedDt = DateTime.Now;

                var hsnOldObj = db.HsnHacCodes.Find(_phsn.HsnHacCodeId);
                db.Entry(hsnOldObj).CurrentValues.SetValues(_phsn);
            }
            try
            {
                db.SaveChanges();
                return "S";
            }
            catch (Exception)
            {
                return "E";
            }
        }
    }
}