﻿using System;
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

        [HttpPost]
        public async Task<ActionResult> LoginSocial(LoginSocialViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.FindByEmail(model.Email);
                if (user == null)
                {
                    var userInfo = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = true,
                        FullName = model.UserName,
                        Avatar = model.Avatar
                    };
                    var result = await _userManager.CreateAsync(userInfo);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        if (user != null)
                            await _userManager.AddToRolesAsync(user.Id, new string[] { "User" });
                    }
                    else
                    {
                        var errors = result.Errors;
                        var message = string.Join(", ", errors);
                        ModelState.AddModelError("", message);
                    }
                }

                var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                Session[CommonConstants.SessionUser] = applicationUserViewModel;
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationProperties props = new AuthenticationProperties();
                props.IsPersistent = false;
                authenticationManager.SignIn(props, identity);
                if (Url.IsLocalUrl(returnUrl))
                {
                }
                else
                {
                    returnUrl = "Dashboard/Index";
                }

                return Json(new
                {
                    returnUrl = returnUrl,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
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
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[CaptchaValidation("CaptchaCode", "Captcha", "Mã xác nhận không đúng")]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userByEmail = await _userManager.FindByEmailAsync(model.Email);
        //        if (userByEmail != null)
        //        {
        //            ModelState.AddModelError("Email", "Email đã tồn tại");
        //         hoặc là nếu email này đã đăng nhập với mạng xã hội trước đó
        //          ModelState.AddModelError("Email", "Email này đã đăng nhập từ mạng xã hội ");
        //            return View(model);
        //        }
        //        var userByUserName = await _userManager.FindByNameAsync(model.UserName);
        //        if (userByUserName != null)
        //        {
        //            ModelState.AddModelError("UserName", "Tài khoản đã tồn tại");
        //            return View(model);
        //        }
        //        var user = new ApplicationUser()
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            EmailConfirmed = true,
        //            BirthDay = DateTime.Now,
        //            FullName = model.FullName,
        //            PhoneNumber = model.PhoneNumber,
        //            Address = model.Address

        //        };

        //        await _userManager.CreateAsync(user, model.Password);


        //        var adminUser = await _userManager.FindByEmailAsync(model.Email);
        //        //if (adminUser != null)
        //        //    await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });

        //        string content = System.IO.File.ReadAllText(Server.MapPath("/assets/client/template/newuser.html"));
        //        content = content.Replace("{{UserName}}", adminUser.FullName);
        //        content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");

        //        MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);

        //        ViewData["SuccessMsg"] = "Đăng ký thành công";



        //        ModelState["FullName"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //        ModelState["Email"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //        ModelState["Address"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //        ModelState["PhoneNumber"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //        ModelState["UserName"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //        ModelState["Password"].Value = new ValueProviderResult("", "", CultureInfo.CurrentCulture);
        //    }

        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Account");
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