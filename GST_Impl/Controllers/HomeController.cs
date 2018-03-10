using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GST_Impl.Controllers
{
    public class HomeController : Controller
    {//Anand  fdfdf
        public ActionResult Index()
        {
            return View();
        }

        public int TestAdd(int a,int b)
        {            
           return a + b;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}