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


			//routes.MapRoute(
			//	name: "NeedSaleAndForRent",
			//	url: "dang-tin-rao-vat-ban-nha-dat.html",
			//	defaults: new { controller = "PostingNews", action = "NeedSaleAndForRent", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);

			//routes.MapRoute(
			//	name: "NeedBuyAndNeedRent",
			//	url: "dang-tin-rao-vat-mua-nha-dat.html",
			//	defaults: new { controller = "PostingNews", action = "NeedBuyAndNeedRent", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//routes.MapRoute(
			//	name: "Login",
			//	url: "dang-nhap.html",
			//	defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);

			//routes.MapRoute(
			//	name: "Register",
			//	url: "dang-ky.html",
			//	defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);

			//routes.MapRoute(
			//	name: "SearchLandNews",
			//	url: "tim-kiem/tin-nha-dat.html",
			//	defaults: new { controller = "LandNews", action = "SearchLandNews", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);

			//routes.MapRoute(
			//	name: "LandNewsByDistrict",
			//	url: "nha-dat/{alias}.d-{id}.html",
			//	defaults: new { controller = "LandNews", action = "LandNewsByDistrict", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//routes.MapRoute(
			//	name: "LandNewsByLandCategory",
			//	url: "nha-dat/{alias}.lc-{id}.html",
			//	defaults: new { controller = "LandNews", action = "LandNewsByLandCategory", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//routes.MapRoute(
			//	name: "LandNewsByLandType",
			//	url: "nha-dat/{alias}.lt-{id}.html",
			//	defaults: new { controller = "LandNews", action = "LandNewsByLandType", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//routes.MapRoute(
			//	name: "LandNewsByProvince",
			//	url: "nha-dat/{alias}.p-{id}.html",
			//	defaults: new { controller = "LandNews", action = "LandNewsByProvince", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//routes.MapRoute(
			//	name: "LandNewsByWard",
			//	url: "nha-dat/{alias}.w-{id}.html",
			//	defaults: new { controller = "LandNews", action = "LandNewsByWard", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);

			//routes.MapRoute(
			//	name: "LandNewsByDetailWithType",
			//	url: "{landtype}/{alias}.l-{id}.html",
			//	defaults: new { controller = "LandNews", action = "Detail", id = UrlParameter.Optional },
			//	  namespaces: new string[] { "LandProject.Web.Controllers" }
			//);
			//         routes.MapRoute(
			//             name: "LandNewsByDetail",
			//             url: "{alias}.l-{id}.html",
			//             defaults: new { controller = "LandNews", action = "Detail", id = UrlParameter.Optional },
			//               namespaces: new string[] { "LandProject.Web.Controllers" }
			//         );

			//         routes.MapRoute(
			//	name: "Default",
			//	url: "{controller}/{action}/{id}",
			//	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
			//	namespaces: new string[] { "LandProject.Web.Controllers" }
			//);


			//version 2
			routes.MapRoute(
			name: "Post",
			url: "tin-tuc.html",
			defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional },
			  namespaces: new string[] { "LandProject.Web.Controllers" }
		);
			routes.MapRoute(
			name: "PostDetail",
			url: "tin-tuc/{alias}.t-{id}.html",
			defaults: new { controller = "Post", action = "Detail", id = UrlParameter.Optional },
			  namespaces: new string[] { "LandProject.Web.Controllers" }
		);

			routes.MapRoute(
                name: "NeedSaleAndForRent",
                url: "dang-tin-mua-ban-nha-dat.html",
                defaults: new { controller = "PostingNews2", action = "Index", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap.html",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );

            routes.MapRoute(
                name: "SearchLandNews",
                url: "tim-kiem/tin-nha-dat.html",
                defaults: new { controller = "LandNews2", action = "SearchLandNews", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky.html",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );

            routes.MapRoute(
                name: "RegisterSuccess",
                url: "dang-ky/thanh-cong.html",
                defaults: new { controller = "Account", action = "RegisterSuccess", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );
			routes.MapRoute(
			  name: "Confirm",
			  url: "confirm.html",
			  defaults: new { controller = "Account", action = "Confirm", id = UrlParameter.Optional },
				namespaces: new string[] { "LandProject.Web.Controllers" }
		  );

			routes.MapRoute(
				name: "LandNewsAllSale",
				url: "nha-dat-ban.html",
				defaults: new { controller = "LandNews2", action = "LandNewsAllSale", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
            routes.MapRoute(
                name: "LandNewsAllRent",
                url: "nha-dat-cho-thue.html",
                defaults: new { controller = "LandNews2", action = "LandNewsAllRent", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );

            routes.MapRoute(
				name: "LandNewsByDistrict",
				url: "nha-dat/{alias}.d-{id}.html",
				defaults: new { controller = "LandNews2", action = "LandNewsByDistrict", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
			routes.MapRoute(
				name: "LandNewsByLandCategory",
				url: "nha-dat/{alias}.lc-{id}.html",
				defaults: new { controller = "LandNews2", action = "LandNewsByLandCategory", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
			routes.MapRoute(
				name: "LandNewsByLandType",
				url: "nha-dat/{alias}.lt-{id}.html",
				defaults: new { controller = "LandNews2", action = "LandNewsByLandType", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
			routes.MapRoute(
				name: "LandNewsByProvince",
				url: "nha-dat/{alias}.p-{id}.html",
				defaults: new { controller = "LandNews2", action = "LandNewsByProvince", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
			routes.MapRoute(
				name: "LandNewsByWard",
				url: "nha-dat/{alias}.w-{id}.html",
				defaults: new { controller = "LandNews2", action = "LandNewsByWard", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);

			routes.MapRoute(
				name: "LandNewsByDetailWithType",
				url: "{landtype}/{alias}.l-{id}.html",
				defaults: new { controller = "LandNews2", action = "Detail", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
			routes.MapRoute(
				name: "LandNewsByDetail",
				url: "{alias}.l-{id}.html",
				defaults: new { controller = "LandNews2", action = "Detail", id = UrlParameter.Optional },
				  namespaces: new string[] { "LandProject.Web.Controllers" }
			);
            routes.MapRoute(
                name: "Manager Member",
                url: "thanh-vien/tin-dang.html",
                defaults: new { controller = "Member", action = "Manager", id = UrlParameter.Optional },
                  namespaces: new string[] { "LandProject.Web.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home2", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "LandProject.Web.Controllers" }
            );
        }
	}
}
