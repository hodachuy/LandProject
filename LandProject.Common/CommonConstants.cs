using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LandProject.Common
{
    public class CommonConstants
    {
        public const string SessionUser = "User";
        public const string Administrator = "Admin";
        public const string Visitor = "Visitor";
        public const string FolderLandNews = "LandNews";
        public const string FolderLProject = "LProject";
		public const string PathMenu = "Menu";
		public const string CreateQnA = "Create";
        public const string UpdateQnA = "Update";
    }

    public class PathServer
    {
        /// <summary>
        /// HttpContext.Current.Server.MapPath("~/File/Images/");
        /// </summary>
        public static string PathImage = HttpContext.Current.Server.MapPath("~/fileman/Uploads/");
    }

    public class PathConfig
    {
        /// <summary>
        /// HttpContext.Current.Server.MapPath("~/Web.config")
        /// </summary>
        public static string PathWebConfig = HttpContext.Current.Server.MapPath("~/Web.config");

        /// <summary>
        /// HttpContext.Current.Server.MapPath("~/AppSettings.config")
        /// </summary>
        public static string PathAppConfig = HttpContext.Current.Server.MapPath("~/AppSettings.config");
    }
}
