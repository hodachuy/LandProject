using AutoMapper;
using LandProject.Web.Infrastructure.Core;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Extensions;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LandProject.Web.API
{
    [RoutePrefix("api/landcategory")]
    public class LandCategoryController : ApiControllerBase
    {
        private ILandCategoryService _landCategoryService;
        public LandCategoryController(IErrorService errorService, ILandCategoryService landCategoryService) : base(errorService)
        {
            _landCategoryService = landCategoryService;
        }

		[Route("getall")]
		[HttpPost]
		public HttpResponseMessage GetAll(HttpRequestMessage request, int lTypeID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if(lTypeID == 0)
					return request.CreateResponse(HttpStatusCode.NoContent);

				var lstLandCategory = _landCategoryService.GetByLandTypeID(lTypeID);
                var lstLandCategoryVm = Mapper.Map<IEnumerable<LandCategory>, IEnumerable<LandCategoryViewModel>>(lstLandCategory);
                response = request.CreateResponse(HttpStatusCode.OK, lstLandCategoryVm);
				return response;
			});
		}

		[Route("getbyid")]
		[HttpGet]
		public HttpResponseMessage GetByID(HttpRequestMessage request, int landCategoryID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (landCategoryID == 0)
					return request.CreateResponse(HttpStatusCode.NoContent);

				var landCategory = _landCategoryService.GetByID(landCategoryID);
                var landCategoryVm = Mapper.Map<LandCategory, LandCategoryViewModel>(landCategory);
                response = request.CreateResponse(HttpStatusCode.OK, landCategoryVm);
				return response;
			});
		}


		[Route("getalltable")]
        [HttpPost]
        public HttpResponseMessage GetAllToTable(HttpRequestMessage request, Request rqFilter)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                int totalRow = 0;
                string filterLCategoryName = "";
                int filterLTypeID = 0;
                rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
                rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
                if (rqFilter.filter != null)
                {
                    foreach(var item in rqFilter.filter.filters)
                    {
                        if(item.Field == "LandTypeID")
                        {
							filterLTypeID = Int32.Parse(item.Value);
                        }
                        if(item.Field == "Name")
                        {
							filterLCategoryName = item.Value;
                        }
                    }
                }
                var lstLandCategory = _landCategoryService.GetAllByCondition(filterLCategoryName, filterLTypeID);
                var lstLandCategoryVm = Mapper.Map<IEnumerable<LandCategory>, IEnumerable<LandCategoryViewModel>>(lstLandCategory);
                totalRow = lstLandCategoryVm.Count();
                var query = lstLandCategoryVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LandCategoryViewModel>()
                {
                    Items = query,
                    Page = rqFilter.page + 1,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / rqFilter.pageSize)
                };
                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

		[Route("create")]
		[HttpPost]
		public HttpResponseMessage Create(HttpRequestMessage request, LandCategoryViewModel lCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LandCategory landCategoryDb = new LandCategory();
				landCategoryDb.UpdateLandCategory(lCategoryVm);
				_landCategoryService.Add(landCategoryDb);
				_landCategoryService.Save();

                var landCategoryVm = Mapper.Map<LandCategory, LandCategoryViewModel>(landCategoryDb);
                response = request.CreateResponse(HttpStatusCode.OK, landCategoryVm);
				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, LandCategoryViewModel lCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LandCategory landCategoryDb = new LandCategory();
				landCategoryDb.UpdateLandCategory(lCategoryVm);
				_landCategoryService.Update(landCategoryDb);
				_landCategoryService.Save();

                var landCategoryVm = Mapper.Map<LandCategory, LandCategoryViewModel>(landCategoryDb);
                response = request.CreateResponse(HttpStatusCode.OK, landCategoryVm);
				return response;
			});
		}
	}
}
