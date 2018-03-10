using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GST_Impl.Models;

namespace GST_Impl.Models
{
    public class Common
    {
        private GSTBookEntities db = new GSTBookEntities();
        public IEnumerable<mst_Uom> GetUMOList()
        {
            return (from u in db.mst_Uom select u).ToList();          
        }

        public IEnumerable<mst_TaxTypes> GetTaxTypeList()
        {
            return (from tt in db.mst_TaxTypes select tt).ToList();
        }

        public IEnumerable<mst_Taxes> GetTaxList()
        {
            return (from tt in db.mst_Taxes select tt).ToList();
            
        }

        public IEnumerable<HsnHacCode> GetHSNList()
        {
            return (from tt in db.HsnHacCodes select tt).ToList();
        }

        public IEnumerable<Customer> GetCustomerListBySearch(string _pStrCustomerName)
        {
            return (from hsn in db.Customers select hsn).ToList();
        }
    }
}