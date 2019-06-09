using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
		public SlideController(IErrorService errorService) : base(errorService)
		{
		}

		// GET: Admin/Slide
		public ActionResult Index()
        {
            return View();
        }
    }
}