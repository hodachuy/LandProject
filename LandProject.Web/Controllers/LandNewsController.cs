using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
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
    public class LandNewsController : Controller
    {
        private ILandNewsService _landNewsService;
        private ILandFileService _landFileService;
        private IAgentService _agentService;
        public LandNewsController(ILandNewsService landNewsService,
            ILandFileService landFileService,
            IAgentService agentService)
        {
            _landNewsService = landNewsService;
            _landFileService = landFileService;
            _agentService = agentService;
        }
        // GET: LandNews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var landNews = _landNewsService.GetByID(id);
            var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

            var lstLandFile = _landFileService.GetByLandNewsID(id);
            var lstLandFileVm = Mapper.Map<IEnumerable<LandFile>, IEnumerable<LandFileViewModel>>(lstLandFile);
            landNewsVm.LandFiles = lstLandFileVm;

            var agent = _agentService.GetByID(landNewsVm.AgentID);
            var agentDb = Mapper.Map<Agent, AgentViewModel>(agent);
            landNewsVm.Agent = agentDb;

            return View(landNewsVm);
        }

		public ActionResult LandNewsByLandType(int landTypeID, int page = 1, string sort = "")
		{
            string filter = "ln.LandTypeID = " + landTypeID;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter,sort, page, pageSize).ToList();
            if(lstLandNews!= null && lstLandNews.Count() != 0)
            {
                totalRow = lstLandNews[0].Total;
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
            {
                Items = lstLandNews,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View();
		}
		public ActionResult LandNewsByLandCategory(int landCategoryID, int page = 1, string sort = "")
		{
            string filter = "ln.LandCategoryID = " + landCategoryID;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
            if (lstLandNews != null && lstLandNews.Count() != 0)
            {
                totalRow = lstLandNews[0].Total;
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
            {
                Items = lstLandNews,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View();
        }
		public ActionResult LandNewsByWard(int wardID, int page = 1, string sort = "")
		{
            string filter = "ln.WardID = " + wardID;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
            if (lstLandNews != null && lstLandNews.Count() != 0)
            {
                totalRow = lstLandNews[0].Total;
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
            {
                Items = lstLandNews,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View();
        }
		public ActionResult LandNewsByDistrict(int districtID, int page = 1, string sort = "")
		{
            string filter = "ln.DistrictID = " + districtID;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
            if (lstLandNews != null && lstLandNews.Count() != 0)
            {
                totalRow = lstLandNews[0].Total;
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
            {
                Items = lstLandNews,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View();
        }
		public ActionResult LandNewsByProvince(int provinceID, int page = 1, string sort = "")
		{
            string filter = "ln.ProvinceID = " + provinceID;
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
            if (lstLandNews != null && lstLandNews.Count() != 0)
            {
                totalRow = lstLandNews[0].Total;
            }

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
            {
                Items = lstLandNews,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View();
        }

		public ActionResult SearchLandNews()
		{
			return View();
		}
	}
}