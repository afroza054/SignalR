using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connection_hub_Persistent_Work_01.Controllers
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

       
        public ActionResult Chat()
        {

            return View();
        }
        public ActionResult GroupChat()
        {
            return View();
        }
        public ActionResult GroupChat2()
        {
            return View();
        }
    }
}