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
        private ILandNewsService _landNewsService;
        public DashboardController(IErrorService errorService,
            ILandTypeService lTypeService,
            IVisitorService visitorService,
            ILandNewsService landNewsService) : base(errorService)
        {
            _lTypeService = lTypeService;
            _visitorService = visitorService;
            _landNewsService = landNewsService;
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            var lstCountLand = _landNewsService.CountLandNewsInLandType().ToList();
            return View(lstCountLand);
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