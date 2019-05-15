using System.Web.Mvc;

namespace LandProject.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "Admin",
                url: "Admin/{controller}/{action}/{id}/{sid}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional, sid = UrlParameter.Optional }
            );
        }
    }
}