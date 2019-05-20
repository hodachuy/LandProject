using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class LandNewsController : Controller
    {
        // GET: Admin/LandNews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int lTypeID)
        {
            ViewBag.LandTypeID = lTypeID;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}