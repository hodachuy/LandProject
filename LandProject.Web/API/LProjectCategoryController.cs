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
using LandProject.Web.Models;
using LandProject.Web.Infrastructure.Extensions;
using AutoMapper;

namespace LandProject.Web.API
{
	[RoutePrefix("api/lprojectcategory")]
	public class LProjectCategoryController : ApiControllerBase
	{
		private ILProjectCategoryService _lProjectCategoryService;
		public LProjectCategoryController(IErrorService errorService, ILProjectCategoryService lProjectCategoryService) : base(errorService)
		{
			_lProjectCategoryService = lProjectCategoryService;
		}

		[Route("getall")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstLProjectCategory = _lProjectCategoryService.GetAll();
                var lstLProjectCategoryVm = Mapper.Map<IEnumerable<LProjectCategory>, IEnumerable<LProjectCategoryViewModel>>(lstLProjectCategory);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectCategoryVm);
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
				string filterLProjectCategoryName = "";
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
				if (rqFilter.filter != null)
				{
					filterLProjectCategoryName = rqFilter.filter.filters[0].Value;
				}
				var lstLProjectCategory = _lProjectCategoryService.GetAllByCondition(filterLProjectCategoryName);
                var lstLProjectCategoryVm = Mapper.Map<IEnumerable<LProjectCategory>, IEnumerable<LProjectCategoryViewModel>>(lstLProjectCategory);
                totalRow = lstLProjectCategoryVm.Count();
				var query = lstLProjectCategoryVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<LProjectCategoryViewModel>()
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

		[Route("getbyid")]
		[HttpGet]
		public HttpResponseMessage GetByID(HttpRequestMessage request, int lProjectCategoryID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (lProjectCategoryID == 0)
				{
					return request.CreateResponse(HttpStatusCode.NoContent);
				}
				var lprjCategory = _lProjectCategoryService.GetByID(lProjectCategoryID);
                var lprjCategoryVm = Mapper.Map<LProjectCategory, LProjectCategoryViewModel>(lprjCategory);
                response = request.CreateResponse(HttpStatusCode.OK, lprjCategoryVm);
				return response;
			});
		}


		[Route("create")]
		[HttpPost]
		public HttpResponseMessage Create(HttpRequestMessage request, LProjectCategoryViewModel lProjectCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LProjectCategory lProjectCategoryDb = new LProjectCategory();
				lProjectCategoryDb.UpdateLProjectCategory(lProjectCategoryVm);
				_lProjectCategoryService.Add(lProjectCategoryDb);
				_lProjectCategoryService.Save();

                var lprjCategoryVm = Mapper.Map<LProjectCategory, LProjectCategoryViewModel>(lProjectCategoryDb);
                response = request.CreateResponse(HttpStatusCode.OK, lprjCategoryVm);
				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, LProjectCategoryViewModel lProjectCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LProjectCategory lProjectCategoryDb = new LProjectCategory();
				lProjectCategoryDb.UpdateLProjectCategory(lProjectCategoryVm);
				_lProjectCategoryService.Update(lProjectCategoryDb);
				_lProjectCategoryService.Save();

                var lprjCategoryVm = Mapper.Map<LProjectCategory, LProjectCategoryViewModel>(lProjectCategoryDb);
                response = request.CreateResponse(HttpStatusCode.OK, lprjCategoryVm);
				return response;
			});
		}
	}
}
