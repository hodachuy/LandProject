using LandProject.Service;
using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class LandCategoryController : BaseController
    {
        public LandCategoryController(IErrorService errorService) : base(errorService)
        {
        }

        // GET: Admin/LandType
        public ActionResult Index()
        {
            return View();
        }
    }
}