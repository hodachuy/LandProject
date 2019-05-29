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
	public class HomeController : Controller
	{
		IMenuGroupService _menugroupService;
		IMenuService _menuService;
        IAddressCommonService _addressCommonService;
        ILandNewsService _landNewsService;

		public HomeController(IMenuGroupService menugroupService,
            IMenuService menuService,
            IAddressCommonService addressCommonService,
            ILandNewsService landNewsService)
		{
			_menugroupService = menugroupService;
			_menuService = menuService;
            _addressCommonService = addressCommonService;
            _landNewsService = landNewsService;
        }

		public ActionResult Index()
		{
            var homeViewModel = new HomeViewModel();
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            string filterSaleRent = "lt.SortOrder < 3" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            var lstLandNewsSaleRent = _landNewsService.GetAllByFilter(filterSaleRent, "PublishedDate desc", 1, 20).ToList();
            var lstLandNewsSaleRentVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNewsSaleRent);

            string filterBuyRent = "lt.SortOrder > 2" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
			var lstLandNewsBuyRent = _landNewsService.GetAllByFilter(filterBuyRent, "PublishedDate desc", 1, 20).ToList();
            var lstLandNewsBuyRentVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNewsBuyRent);

            homeViewModel.CategoryWard = categoryWard;
            homeViewModel.LandNewsSaleAndRent = lstLandNewsSaleRentVm;
            homeViewModel.LandNewsBuyAndRent = lstLandNewsBuyRentVm;
            return View(homeViewModel);
		}

        [ChildActionOnly]
        public ActionResult Header()
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
        public ActionResult Footer()
        {
			var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
			ViewBag.CategoryFooter = categoryWard;
			return PartialView();
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
