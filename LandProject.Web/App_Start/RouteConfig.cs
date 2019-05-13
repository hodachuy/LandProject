using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LandProject.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


			routes.MapRoute(
				name: "NeedSaleAndForRent",
				url: "dang-tin-rao-vat-ban-nha-dat.html",
				defaults: new { controller = "PostingNews", action = "NeedSaleAndForRent", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);

			routes.MapRoute(
				name: "NeedBuyAndNeedRent",
				url: "dang-tin-rao-vat-mua-nha-dat.html",
				defaults: new { controller = "PostingNews", action = "NeedBuyAndNeedRent", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
