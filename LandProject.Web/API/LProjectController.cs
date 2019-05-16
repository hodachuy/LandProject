using BotProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Web.Infrastructure.Core;

namespace LandProject.Web.API
{
	public class LProjectController : ApiControllerBase
	{
		private ILProjectService _lProjectService;
		public LProjectController(IErrorService errorService, ILProjectService lProjectService) : base(errorService)
		{
			_lProjectService = lProjectService;
		}

		[Route("getall")]
		[HttpPost]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstLProject = _lProjectService.GetAll();
				response = request.CreateResponse(HttpStatusCode.OK, lstLProject);
				return response;
			});
		}

		[Route("getbylprojectcategory")]
		[HttpPost]
		public HttpResponseMessage GetAllByLProjectCategory(HttpRequestMessage request, int lProjectCategoryID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (lProjectCategoryID == 0)
					return request.CreateResponse(HttpStatusCode.NoContent);

				var lstLProject = _lProjectService.GetByLProjectCategoryID(lProjectCategoryID);
				response = request.CreateResponse(HttpStatusCode.OK, lstLProject);
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
				var lstLandType = _lProjectService.GetAllByCondition(filterLProjectName, filterLTypeID);
				totalRow = lstLandType.Count();
				var query = lstLandType.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<LProject>()
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
