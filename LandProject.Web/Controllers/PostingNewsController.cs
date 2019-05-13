using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
	public class PostingNewsController : Controller
	{
		// GET: News
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult NeedSaleAndForRent()
		{
			return View();
		}
		public ActionResult NeedBuyAndNeedRent()
		{
			return View();
		}
	}
}