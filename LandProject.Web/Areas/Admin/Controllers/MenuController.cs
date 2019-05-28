using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(IErrorService errorService) : base(errorService)
        {
        }

        // GET: Admin/Menu
        public ActionResult MenuParent()
        {
            return View();
        }
		public ActionResult MenuChildren()
		{
			return View();
		}
	}
}