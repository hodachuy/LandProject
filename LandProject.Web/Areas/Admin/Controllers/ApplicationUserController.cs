using LandProject.Web.Areas.Admin.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandProject.Service;
using Microsoft.AspNet.Identity;
using LandProject.Model.Models;
using AutoMapper;
using LandProject.Web.Models;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class ApplicationUserController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserController(IErrorService errorService, ApplicationUserManager userManager) : base(errorService)
        {
            _userManager = userManager;
        }

        // GET: Admin/ApplicationUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult AccountLandNews(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var user = _userManager.FindById(userId);
            var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return View(applicationUserViewModel);
        }
    }
}