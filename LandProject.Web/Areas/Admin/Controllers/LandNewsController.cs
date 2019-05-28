using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class LandNewsController : BaseController
    {
        public LandNewsController(IErrorService errorService) : base(errorService)
        {
        }

        // GET: Admin/LandNews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int lTypeID, string lTypeName)
        {
            ViewBag.LandTypeID = lTypeID;
			ViewBag.LandTypeName = lTypeName;
			return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}