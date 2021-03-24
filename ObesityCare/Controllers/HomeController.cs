using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ObesityCare.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
        public ActionResult BMI()
        {
            ViewBag.Message = "BMI Calculator";

            return View();
        }
        public ActionResult Activities()
        {
            ViewBag.Message = "Recommended Activities";

            return View();
        }
    }
}