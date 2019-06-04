﻿using LandProject.Web.Areas.Admin.Infrastructure.Core;
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

        public Home2Controller(IMenuGroupService menugroupService,
        IMenuService menuService,
        IAddressCommonService addressCommonService,
        ILandNewsService landNewsService)
        {
            _menugroupService = menugroupService;
            _menuService = menuService;
            _addressCommonService = addressCommonService;
            _landNewsService = landNewsService;
        }
        // GET: Home2
        public ActionResult Index()
        {
            return View();
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