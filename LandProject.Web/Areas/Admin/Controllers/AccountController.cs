using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LandProject.Model.Models;
using LandProject.Web.Models;
using LandProject.Common;
using System.Web.Mvc;
using AutoMapper;
using LandProject.Web.Infrastructure.Core;
using LandProject.Service;
using System.Linq;

namespace LandProject.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AccountController()
        {
        }



        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            // check login with remember me
            if (Request.IsAuthenticated)
            {
                if (Session[CommonConstants.SessionUser] == null)
                {
                    ApplicationUser user = _userManager.FindById(User.Identity.GetUserId());
                    var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                    Session[CommonConstants.SessionUser] = applicationUserViewModel;
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                    var listGroup = applicationGroupService.GetListGroupByUserId(user.Id).ToList();
                    if ((listGroup.Count == 0) || ((listGroup.Count) != 0 && (listGroup.Any(x => x.Name != CommonConstants.Administrator))))
                    {
                        ModelState.AddModelError("", "Bạn không có quyền truy cập vào trang quản trị.");
                        return View(model);
                    }
                    var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                    Session[CommonConstants.SessionUser] = applicationUserViewModel;
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateUserInfo(ApplicationUserViewModel userVm)
        {
			var userDb = _userManager.FindById(userVm.Id);

			var userCheckEmail = _userManager.FindByEmail(userVm.Email);
            if(userCheckEmail != null)
            {
				if(userDb.Email != userVm.Email) {
					return Json(new
					{
						message = "Email này đã được đăng ký từ tài khoản khác",
						status = false
					});
				}

            }
            var userCheckUserName = _userManager.FindByName(userVm.UserName);
            if (userCheckUserName != null)
            {
				if(userDb.UserName != userVm.UserName)
				{
					return Json(new
					{
						message = "Tên tài khoản đã được đăng ký từ tài khoản khác",
						status = false
					});
				}

            }

            userDb.FullName = userVm.FullName;
            userDb.UserName = userVm.UserName;
            userDb.PhoneNumber = userVm.PhoneNumber;
            userDb.Address = userVm.Address;
            userDb.Email = userVm.Email;

            _userManager.Update(userDb);

            var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(userDb);
            Session[CommonConstants.SessionUser] = applicationUserViewModel;

            return Json(new
            {
                message = "Chỉnh sửa thành công",
                status = true
            });
        }

        [HttpPost]
        public JsonResult UpdatePassword(string userId, string passwordCurrent, string passwordNew)
        {
            var userDb = _userManager.FindById(userId);
            var checkUser = _userManager.Find(userDb.UserName, passwordCurrent);
            if(checkUser != null)
            {
				_userManager.RemovePassword(userId);
				_userManager.AddPassword(userId, passwordNew);
            }else
            {
                return Json(new
                {
                    message = "Mật khẩu cũ không đúng",
                    status = false
                });
            }          
            return Json(new
            {
                message = "Chỉnh sửa thành công",
                status = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            return Json(new
            {
                message = "success.",
                status = true
            });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}