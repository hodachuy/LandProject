using AutoMapper;
using LandProject.Common;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Models;
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

        public MemberController(ILandNewsService landNewsService)
        {
            _landNewsService = landNewsService;
        }
        // GET: Member
        public ActionResult Index()
        {
            return View();
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