using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GST_Impl.Models;

namespace GST_Impl.Controllers
{
    public class ClientController : Controller
    {
        private GSTBookEntities db = new GSTBookEntities();
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }

        public string Init()
        {
            Customer customer = new Customer();           
            customer.GUID = Guid.NewGuid().ToString();
            customer.CrudStatus = "I";

            var CustomerList = (from c in db.Customers select new { c.CustomerId,c.GUID,c.CompanyIdNo,c.CompanyName}).ToList();

            string[] detailArr = new string[1];

            var details = (from d in detailArr
                           select new
                           {
                               customer = customer,
                               CustomerList = CustomerList
                           }).FirstOrDefault();

            return (Newtonsoft.Json.JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None,
                new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string Edit(string _pGUID)
        {
            var customer = (from c in db.Customers where c.GUID == _pGUID select new
            {
                c.CustomerId,
                c.CompanyName,
                c.Name,
                c.LocAddress,
                c.CountryId,
                c.StateId,
                c.CityId,
                c.CompanyIdNo,
                c.GSTN,
                c.Telephone,
                c.Pincode,
                c.Remarks,
                c.CreatedDt,
                c.CreatedBy,
                c.UpdatedDt,
                c.UpdatedBy,
                CrudStatus = "U",
                CustomerContacts = c.CustomerContacts.Select(s => new { s.ContactId, s.CustomerId , s.ContactName, s.ContactNo , s.EmailId, CrudStatus = "U"}),
                DeletedCustomerContactDetails = ""
            }).FirstOrDefault();
            
            return (Newtonsoft.Json.JsonConvert.SerializeObject(customer, Newtonsoft.Json.Formatting.None,
              new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }

        public string Save(Customer _paramCustomer)
        {
            if (_paramCustomer.CrudStatus == "I")
            {
                // _pCustomer.CreatedBy = "Logged In User";
                _paramCustomer.CreatedDt = DateTime.Now;
                db.Customers.Add(_paramCustomer);
            }
            else
            {
                var oldCustomerObj = db.Customers.Find(_paramCustomer.CustomerId);

                // _pCustomer.UpdatedBy = "Logged In User";
                _paramCustomer.UpdatedDt = DateTime.Now;

                db.Entry<Customer>(oldCustomerObj).CurrentValues.SetValues(_paramCustomer);


                foreach (var item in oldCustomerObj.CustomerContacts)
                {
                    if (item.CrudStatus == "I")
                    {
                        db.CustomerContacts.Add(item);
                    }
                    else
                    {
                        var oldCustomerContactDetail = db.CustomerContacts.Find(item.ContactId);
                        db.Entry<CustomerContact>(oldCustomerContactDetail).CurrentValues.SetValues(item);
                    }

                }

                if (_paramCustomer.DeletedCustomerContactDetails != null)
                {
                    if (_paramCustomer.DeletedCustomerContactDetails.Count > 0)
                    {
                        foreach (var item in _paramCustomer.DeletedCustomerContactDetails)
                        {
                            var oldCustomerContactDetail = db.CustomerContacts.Find(item.ContactId);
                            db.CustomerContacts.Remove(oldCustomerContactDetail);
                        }
                    }
                }

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

        
      [HttpPost]
        public string GetClientList()
        {
            var customers = db.Customers.Select(s => new { s.CustomerId, s.CompanyName }).ToList();
            return (Newtonsoft.Json.JsonConvert.SerializeObject(customers, Newtonsoft.Json.Formatting.None,
            new Newtonsoft.Json.JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore })).ToString();
        }
    }
}