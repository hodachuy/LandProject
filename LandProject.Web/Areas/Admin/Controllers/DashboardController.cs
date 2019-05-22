using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private ILandTypeService _lTypeService;
        private IVisitorService _visitorService;
        public DashboardController(IErrorService errorService,
            ILandTypeService lTypeService,
            IVisitorService visitorService) : base(errorService)
        {
            _lTypeService = lTypeService;
            _visitorService = visitorService;
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView(UserInfo);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var lstLandType = _lTypeService.GetAll();
            var totalVisitor = _visitorService.CountVisitor();
            ViewBag.TotalVisitor = totalVisitor;
            return PartialView(lstLandType);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}