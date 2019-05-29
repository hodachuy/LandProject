using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
    public class LandNewsController : Controller
    {
        private ILandNewsService _landNewsService;
        private ILandFileService _landFileService;
        private IAddressCommonService _addressCommonService;
        private ILandTypeService _landTypeService;
        private ILandCategoryService _landCategoryService;
        private IAgentService _agentService;
        public LandNewsController(ILandNewsService landNewsService,
            ILandFileService landFileService,
            IAgentService agentService,
            IAddressCommonService addressCommonService,
            ILandTypeService landTypeService,
            ILandCategoryService landCategoryService)
        {
            _landNewsService = landNewsService;
            _landFileService = landFileService;
            _agentService = agentService;
            _addressCommonService = addressCommonService;
            _landTypeService = landTypeService;
            _landCategoryService = landCategoryService;
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

			LandNewsDetailViewModel landDetail = new LandNewsDetailViewModel();
			landDetail.LandNewsDetail = landNewsVm;

			string filter = "ln.LandTypeID = " + landNewsVm.LandTypeID + " and ln.IsDelete = 0 and ln.IsPublished = 1 and ln.ID != "+landNewsVm.ID + " and ln.DistrictID = "+ landNewsVm.DistrictID;
			var landRelated = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", 1, 5).ToList();
			var lstLandNewsRelatedVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(landRelated);
			landDetail.LandNewsRelated = lstLandNewsRelatedVm;
			return View(landDetail);
        }

		public ActionResult LandNewsByLandType(int id, int page = 1, string sort = "")//landTypeID
        {
            var landTypeDetail = _landTypeService.GetByID(id);
            ViewBag.LandTypeName = landTypeDetail.Name;

            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            ViewBag.CategoryWard = categoryWard;
            string filter = "ln.LandTypeID = " + id + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize).ToList();
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
		public ActionResult LandNewsByLandCategory(int id, int page = 1, string sort = "")//landCategoryID
        {
            var landCategoryDetail = _landCategoryService.GetByID(id);
            ViewBag.LandTypeName = landCategoryDetail.LandType.Name;
            ViewBag.LandCategoryName = landCategoryDetail.Name;
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            ViewBag.CategoryWard = categoryWard;
            string filter = "ln.LandCategoryID = " + id + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize).ToList();
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
		public ActionResult LandNewsByWard(int id, int page = 1, string sort = "")//wardID
        {
            var wardDetail = _addressCommonService.GetDetailWardByID(id);
            ViewBag.WardName = wardDetail.Name;
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            ViewBag.CategoryWard = categoryWard;
            string filter = "ln.WardID = " + id + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize);
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
		public ActionResult LandNewsByDistrict(int id, int page = 1, string sort = "")//districtID
        {
            var districtDetail = _addressCommonService.GetDetailDistrictByID(id);
            ViewBag.DistrictName = districtDetail.Name;
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            ViewBag.CategoryWard = categoryWard;
            string filter = "ln.DistrictID = " + id + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize);
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
		public ActionResult LandNewsByProvince(int id, int page = 1, string sort = "")//provinceID
        {
            var provinceDetail = _addressCommonService.GetDetailProvinceByID(id);
            ViewBag.ProvinceName = provinceDetail.Name;
            var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
            ViewBag.CategoryWard = categoryWard;
            string filter = "ln.ProvinceID = " + id + " and ln.IsDelete = 0 and ln.IsPublished = 1";
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize).ToList();
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

		public ActionResult SearchLandNews(int landtype, int province, string minprice,string maxprice, string minarea,string maxarea, int page = 1, string sort = "")
		{
			string filter = "ln.IsDelete = 0 and ln.IsPublished = 1";
			if (landtype != 0)
			{
				filter += " and ln.LandTypeID = " + landtype;
			}
			if (province != 0)
			{
				filter += " and ln.ProvinceID = " + province;
			}
			if (!String.IsNullOrEmpty(minprice))
			{
				minprice = Regex.Replace(minprice, ",", "");
				decimal price = decimal.Parse(minprice);
				filter += " and ln.Price > " + price;
			}
			if (!String.IsNullOrEmpty(maxprice))
			{
				maxprice = Regex.Replace(maxprice, ",", "");
				decimal price = decimal.Parse(maxprice);
				filter += " and ln.Price < " + price;
			}
			if (!String.IsNullOrEmpty(minarea))
			{
				int area = Int32.Parse(minarea);
				filter += " and ln.Area > " + area;
			}
			if (!String.IsNullOrEmpty(maxarea))
			{
				int area = Int32.Parse(maxarea);
				filter += " and ln.Area < " + area;
			}

			var categoryWard = _addressCommonService.GetTotalLandNewsOfWards(571).ToList();
			ViewBag.CategoryWard = categoryWard;

			int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
			int totalRow = 0;
			var lstLandNews = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", page, pageSize).ToList();
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
	}
}