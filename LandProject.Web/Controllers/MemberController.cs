using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
    public class MemberController : Controller
    {
        ILandNewsService _landNewsService;
		private ApplicationUserManager _userManager;


		public MemberController(ILandNewsService landNewsService, ApplicationUserManager userManager)
        {
            _landNewsService = landNewsService;
			_userManager = userManager;

		}
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult ManagerAccount()
		{
			ViewBag.User = UserInfo;
			return View();
		}

		public JsonResult UpdateAccount(ApplicationUserViewModel user)
		{
			ViewBag.User = UserInfo;
			var userDb = _userManager.FindByEmail(user.Email);
			userDb.Address = user.Address;
			userDb.PhoneNumber = user.PhoneNumber;
			userDb.MobilePhone = user.PhoneNumber;
			userDb.FullName = user.FullName;
			_userManager.Update(userDb);

			var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(userDb);
			Session[CommonConstants.SessionUser] = applicationUserViewModel;

			return Json(new
			{
				message = "OK",
				status = true
			});
		}

		public ActionResult Manager()
        {
            string UserId = UserInfo.Id;
            int page = 1;
            int pageSize = 1000;
            int totalRow = 0;

            string filter = "ln.UserID like '%"+UserId+"%'";

            var lstLandNews = _landNewsService.GetAllByFilter(filter, null, page, pageSize).ToList();
            var lstLandNewsVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(lstLandNews);
            if (lstLandNewsVm != null && lstLandNewsVm.Count() != 0)
            {
                totalRow = lstLandNewsVm.Count();
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsViewModel>()
            {
                Items = lstLandNewsVm,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
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