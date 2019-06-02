using LandProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;
using LandProject.Common;
using LandProject.Model.Models;
using AutoMapper;
using LandProject.Web.Models;

namespace LandProject.Web.API
{
    [RoutePrefix("api/lproject")]
    public class LProjectController : ApiControllerBase
	{
		private ILProjectService _lProjectService;
		public LProjectController(IErrorService errorService, ILProjectService lProjectService) : base(errorService)
		{
			_lProjectService = lProjectService;
		}

		[Route("getall")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstLProject = _lProjectService.GetAll();
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
				return response;
			});
		}

		[Route("getbylproject")]
		[HttpGet]
		public HttpResponseMessage GetAllByLProjectCategory(HttpRequestMessage request, int lProjectCategoryID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (lProjectCategoryID == 0)
					return request.CreateResponse(HttpStatusCode.NoContent);

				var lstLProject = _lProjectService.GetByLProjectCategoryID(lProjectCategoryID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
				return response;
			});
		}

        [Route("getbydistrict")]
        [HttpGet]
        public HttpResponseMessage GetAllByDistrict(HttpRequestMessage request, int districtID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (districtID == 0)
                    return request.CreateResponse(HttpStatusCode.NoContent);

                var lstLProject = _lProjectService.GetByDistrictID(districtID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
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
				string filterLProjectName = "";
				int filterLTypeID = 0;
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
				if (rqFilter.filter != null)
				{
					foreach (var item in rqFilter.filter.filters)
					{
						if (item.Field == "LProjectCategoryID")
						{
							filterLTypeID = Int32.Parse(item.Value);
						}
						if (item.Field == "LProjectName")
						{
							filterLProjectName = item.Value;
						}
					}
				}
				var lstLProject = _lProjectService.GetAllByCondition(filterLProjectName, filterLTypeID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                totalRow = lstLProjectVm.Count();
				var query = lstLProjectVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<LProjectViewModel>()
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
	}
}
