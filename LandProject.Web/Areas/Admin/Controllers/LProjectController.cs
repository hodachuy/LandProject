using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class LProjectController : Controller
    {
        // GET: Admin/LProject
        public ActionResult Index()
        {
            return View();
        }
    }
}