using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;
using LandProject.Common;
using LandProject.Web.Models;
using AutoMapper;
using LandProject.Model.Models;

namespace LandProject.Web.Controllers
{
    public class Home2Controller : Controller
    {
        IMenuGroupService _menugroupService;
        IMenuService _menuService;
        IAddressCommonService _addressCommonService;
        ILandNewsService _landNewsService;
		ICommonService _commonService;

        public Home2Controller(IMenuGroupService menugroupService,
        IMenuService menuService,
        IAddressCommonService addressCommonService,
		ICommonService commonService,
		ILandNewsService landNewsService)
        {
            _menugroupService = menugroupService;
            _menuService = menuService;
            _addressCommonService = addressCommonService;
            _landNewsService = landNewsService;
			_commonService = commonService;

		}
        // GET: Home2
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();

			var slide = _commonService.GetSlide().ToList();
			ViewBag.Slide = slide;


			int pageSizeHot = int.Parse(ConfigHelper.GetByKey("PageSizeHot"));
			int pageSizeSale = int.Parse(ConfigHelper.GetByKey("PageSizeSale"));
			int pageSizeRent = int.Parse(ConfigHelper.GetByKey("PageSizeRent"));
			int keyLandHot = int.Parse(ConfigHelper.GetByKey("LandScheduleID"));

            //tin noi bat
            string filterHot = "ln.LandNewsScheduleID = "+ keyLandHot + "" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            var lstLandNewsHot = _landNewsService.GetAllByFilter(filterHot, "PublishedDate desc", 1, pageSizeHot).ToList();
            var lstLandNewsHotVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNewsHot);


            string filterSaleRent = "lt.TypeExchange = 1" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            var lstLandNewsSaleRent = _landNewsService.GetAllByFilter(filterSaleRent, "PublishedDate desc", 1, pageSizeSale).ToList();
            var lstLandNewsSaleRentVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNewsSaleRent);


            string filterBuyRent = "lt.TypeExchange = 2" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            var lstLandNewsBuyRent = _landNewsService.GetAllByFilter(filterBuyRent, "PublishedDate desc", 1, pageSizeRent).ToList();
            var lstLandNewsBuyRentVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNewsBuyRent);

            homeViewModel.CategoryWard = categoryWard;
            homeViewModel.LandNewsHot = lstLandNewsHotVm;
            homeViewModel.LandNewsSaleAndRent = lstLandNewsSaleRentVm;
            homeViewModel.LandNewsBuyAndRent = lstLandNewsBuyRentVm;
            return View(homeViewModel);
        }


        [ChildActionOnly]
        public ActionResult Header2()
        {
            var lstMenuGroup = _menugroupService.GetMenuGroupActive();
            var lstMenuGroupVm = Mapper.Map<IEnumerable<MenuGroup>, IEnumerable<MenuGroupViewModel>>(lstMenuGroup).OrderBy(x => x.DisplayOrder);
            foreach (var item in lstMenuGroupVm)
            {
                var menu = _menuService.GetMenuActiveByMenuGroup(item.ID);
                item.Menus = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(menu);
            }
            ViewBag.UserInfo = UserInfo;
            return PartialView(lstMenuGroupVm);
        }

        [ChildActionOnly]
        public ActionResult Footer2()
        {
            return PartialView();
        }

		[ChildActionOnly]
		[OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
		public ActionResult CategoryDistrict()
		{
			var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
			return PartialView(categoryWard);
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