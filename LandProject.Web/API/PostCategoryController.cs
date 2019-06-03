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
	[RoutePrefix("api/postcategory")]
	public class PostCategoryController : ApiControllerBase
	{
		IPostCategoryService _postCategoryService;
		public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService) : base(errorService)
		{
			_postCategoryService = postCategoryService;
		}

		[Route("getall")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstPostCategory = _postCategoryService.GetAll();
				var lstPostCategoryVm = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(lstPostCategory);
				response = request.CreateResponse(HttpStatusCode.OK, lstPostCategoryVm);
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
				string filterLTypeName = "";
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
				if (rqFilter.filter != null)
				{
					filterLTypeName = rqFilter.filter.filters[0].Value;
				}
				var lstPostCategory = _postCategoryService.GetAllByCondition(filterLTypeName);
				var lstPostCategoryVm = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(lstPostCategory);
				totalRow = lstPostCategoryVm.Count();
				var query = lstPostCategoryVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<PostCategoryViewModel>()
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
		public HttpResponseMessage GetByID(HttpRequestMessage request, int postCategoryID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (postCategoryID == 0)
				{
					return request.CreateResponse(HttpStatusCode.NoContent);
				}
				var postCategory = _postCategoryService.GetById(postCategoryID);
				var postCategoryVm = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategory);
				response = request.CreateResponse(HttpStatusCode.OK, postCategoryVm);
				return response;
			});
		}


		[Route("create")]
		[HttpPost]
		public HttpResponseMessage Create(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				PostCategory postCategoryDb = new PostCategory();
				postCategoryDb.UpdatePostCategory(postCategoryVm);
				postCategoryDb.CreatedDate = DateTime.Now;

				_postCategoryService.Add(postCategoryDb);
				_postCategoryService.Save();
				var postCategoryD = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategoryDb);
				response = request.CreateResponse(HttpStatusCode.OK, postCategoryD);
				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				PostCategory postCategoryDb = new PostCategory();
				postCategoryDb.UpdatePostCategory(postCategoryVm);
				postCategoryDb.UpdatedDate = DateTime.Now;
			

				_postCategoryService.Update(postCategoryDb);
				_postCategoryService.Save();
				var postCategoryD = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategoryDb);
				response = request.CreateResponse(HttpStatusCode.OK, postCategoryD);
				return response;
			});
		}
	}
}
