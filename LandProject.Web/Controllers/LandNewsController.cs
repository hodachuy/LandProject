using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
    public class LandNewsController : Controller
    {
        // GET: LandNews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            return View();
        }

		public ActionResult LandNewsByLandType(int landTypeID)
		{
			return View();
		}
		public ActionResult LandNewsByLandCategory(int landCategoryID)
		{
			return View();
		}
		public ActionResult LandNewsByWard(int wardID)
		{
			return View();
		}
		public ActionResult LandNewsByDistrict(int districtID)
		{
			return View();
		}
		public ActionResult LandNewsByProvince(int provinceID)
		{
			return View();
		}

		public ActionResult SearchLandNews()
		{
			return View();
		}
	}
}