using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
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