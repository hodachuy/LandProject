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

namespace LandProject.Web.Controllers
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

				return RedirectToAction("Index", "Home2");
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
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Tài khoản bạn chưa được kích hoạt, vui lòng vào hợp thư mail "+user.Email+" để kích hoạt tài khoản.");
                        return View(model);
                    }
					var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
					Session[CommonConstants.SessionUser] = applicationUserViewModel;
					IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
					authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
					ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
					AuthenticationProperties props = new AuthenticationProperties();
					props.IsPersistent = true;// model.RememberMe;
					authenticationManager.SignIn(props, identity);
					if (Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home2");
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
		public async Task<ActionResult> LoginSocial(LoginSocialViewModel model)
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
				return Json(new
				{
					returnUrl = "/thanh-vien/ho-so.html",
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

		[HttpPost]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userByUserName = await _userManager.FindByNameAsync(model.UserName);
				if (userByUserName != null)
				{
					ModelState.AddModelError("UserName", "Tài khoản đã tồn tại");
					return View(model);
				}

				var userByEmail = await _userManager.FindByEmailAsync(model.Email);
				if (userByEmail != null)
				{
					ModelState.AddModelError("Email", "Email này đã được đăng ký");
					return View(model);
				}

				var user = new ApplicationUser()
				{
					UserName = model.UserName,
					Email = model.Email,
					EmailConfirmed = false,
					BirthDay = DateTime.Now,
					FullName = model.FullName,
					PhoneNumber = model.PhoneNumber,
					Address = model.Address

				};

				await _userManager.CreateAsync(user, model.Password);


				var newUser = await _userManager.FindByEmailAsync(model.Email);
				if (newUser != null)
				    await _userManager.AddToRolesAsync(newUser.Id, new string[] { "User" });

				string content = System.IO.File.ReadAllText(Server.MapPath("/assets/client_v2/template/newuser.html"));
				content = content.Replace("{{UserName}}", newUser.FullName);
				content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "confirm.html?email="+newUser.Email);

				MailHelper.SendMail(newUser.Email, "Kích hoạt tài khoản tại Datdackhu", content);

				return RedirectToAction("RegisterSuccess", "Account",new { name= user.FullName, mail = user.Email });

			}

			return View();
		}

		public ActionResult Confirm(string email)
		{
			var user = _userManager.FindByEmail(email);
			if(user != null)
			{
				var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
				Session[CommonConstants.SessionUser] = applicationUserViewModel;
				IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
				authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
				ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
				AuthenticationProperties props = new AuthenticationProperties();
				props.IsPersistent = true;// model.RememberMe;
				authenticationManager.SignIn(props, identity);

				if (user.EmailConfirmed)
				{
					return RedirectToAction("Index", "Home2");
				}else
				{
					user.EmailConfirmed = true;
					_userManager.Update(user);
				}
			}
			return View();
		}

		[HttpPost]
		public JsonResult ResetPassword(string email)
		{
			var user = _userManager.FindByEmail(email);
			if(user != null)
			{
				string content = System.IO.File.ReadAllText(Server.MapPath("/assets/client_v2/template/resetpassword.html"));
				content = content.Replace("{{UserName}}", user.FullName);
				try
				{
					MailHelper.SendMail(user.Email, "Cấp lại mật khẩu tài khoản tại Datdackhu", content);
				}catch(Exception ex)
				{
					return Json(new
					{
						message = "Hệ thống gửi mail có vấn đề xin vui lòng quay lại sau",
						status = false
					});
				}
				_userManager.ChangePassword(user.Id, user.PasswordHash, "123@abc");
			}else
			{
				return Json(new
				{
					message = "Không tìm thấy tài khoản trong hệ thống!",
					status = false
				});
			}
			return Json(new
			{
				message = "OK",
				status = true
			});
		}


		public ActionResult RegisterSuccess(string name, string mail)
        {
            ViewBag.Name = name;
            ViewBag.Email = mail;
            return View();
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
