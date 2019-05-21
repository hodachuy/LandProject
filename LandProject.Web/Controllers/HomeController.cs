using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;
using LandProject.Common;
using LandProject.Web.Models;

namespace LandProject.Web.Controllers
{
	public class HomeController : Controller
	{

        public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

        [ChildActionOnly]
        public ActionResult Header()
        {
			return PartialView(UserInfo);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }

		public ApplicationUserViewModel UserInfo
		{
			get
			{
				if (Session != null)
				{
					if (Session[CommonConstants.SessionUser] != null)
					{
						return (ApplicationUserViewModel)Session[CommonConstants.SessionUser];
					}
				}
				return null;
			}
			set
			{
				if (value == null)
					Session.Remove(CommonConstants.SessionUser);
				else
					Session[CommonConstants.SessionUser] = value;
			}
		}
	}
}
