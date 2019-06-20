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
    public class LandNews2Controller : Controller
    {
		private ILandNewsService _landNewsService;
		private ILandFileService _landFileService;
		private IAddressCommonService _addressCommonService;
		private ILandTypeService _landTypeService;
		private ILandCategoryService _landCategoryService;
		private IAgentService _agentService;

		public LandNews2Controller(ILandNewsService landNewsService,
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

		// GET: LandNews2
		public ActionResult Detail(int id)
        {
            var landNews = _landNewsService.GetAllByFilter("ln.ID = " + id, null, 1, 1).ToList()[0];
            var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

            var lstLandFile = _landFileService.GetByLandNewsID(id);
			var lstLandFileVm = Mapper.Map<IEnumerable<LandFile>, IEnumerable<LandFileViewModel>>(lstLandFile);
			landNewsVm.LandFiles = lstLandFileVm;

			var agent = _agentService.GetByID(landNewsVm.AgentID);
			var agentDb = Mapper.Map<Agent, AgentViewModel>(agent);
			landNewsVm.Agent = agentDb;

			LandNewsDetailViewModel landDetail = new LandNewsDetailViewModel();
			landDetail.LandNewsDetail = landNewsVm;

			//tin lien quan
			string filter = "ln.LandTypeID = " + landNewsVm.LandTypeID + " and ln.IsDelete = 0 and ln.IsPublished = 1 and ln.ID != " + landNewsVm.ID + " and ln.DistrictID = " + landNewsVm.DistrictID;
			var landRelated = _landNewsService.GetAllByFilter(filter, "PublishedDate desc", 1, 5).ToList();

			var lstLandNewsRelatedVm = Mapper.Map<IEnumerable<LandNewsFilterViewModel>, IEnumerable<LandNewsViewModel>>(landRelated);
			landDetail.LandNewsRelated = lstLandNewsRelatedVm;
			return View(landDetail);
		}

		//lay tat ca TypeExchange = 1 ben ban'
		public ActionResult LandNewsAllSale(int page = 1, string sort = "")//landTypeID
		{
			var landType = _landTypeService.GetAll().ToList();
			ViewBag.LandType = landType;

			string filter = "lt.TypeExchange = 1" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
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

        //lay tat ca TypeExchange = 1 ben ban'
        public ActionResult LandNewsAllRent(int page = 1, string sort = "")//landTypeID
        {
			var landType = _landTypeService.GetAll().ToList();
			ViewBag.LandType = landType;

			string filter = "lt.TypeExchange = 2" + " and ln.IsDelete = 0 and ln.IsPublished = 1";
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



        public ActionResult LandNewsByLandType(int id, int page = 1, string sort = "")//landTypeID
		{
			var landTypeDetail = _landTypeService.GetByID(id);
			ViewBag.LandTypeName = landTypeDetail.Name;
			ViewBag.LandTypeAlias = landTypeDetail.Alias;

			var landCategory = _landCategoryService.GetByLandTypeID(id).ToList();
			ViewBag.LandCategory = landCategory;

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
			ViewBag.LandTypeAlias = landCategoryDetail.LandType.Alias;
			ViewBag.LandTypeID = landCategoryDetail.LandType.ID;
			ViewBag.LandCategoryName = landCategoryDetail.Name;

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

        public ActionResult SearchLandNews(string address, int landType, int landCategory, string price, int page = 1, string sort = "")
        {
            string filter = "ln.IsDelete = 0 and ln.IsPublished = 1";
            if (!String.IsNullOrEmpty(address))
            {
                filter += " and ln.Address like '%"+ address + "' ";
            }
            if (landType != 0)
            {
                filter += " and ln.LandTypeID = " + landType;
            }
            if (landCategory != 0)
            {
                filter += " and ln.LandCategoryID = " + landCategory;
            }
            if (!String.IsNullOrEmpty(price))
            {
                decimal priceVal = decimal.Parse(price);
                filter += " and ln.DecimalTotalPrice > " + priceVal;
            }

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