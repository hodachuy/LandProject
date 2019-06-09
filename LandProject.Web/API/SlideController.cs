using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Infrastructure.Extensions;
using LandProject.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LandProject.Web.API
{
	[RoutePrefix("api/slide")]
	public class SlideController : ApiControllerBase
	{
		ICommonService _commomService;
		public SlideController(IErrorService errorService, ICommonService commomService) : base(errorService)
		{
			_commomService = commomService;
		}
		[Route("getalltable")]
		[HttpPost]
		public HttpResponseMessage GetAllTable(HttpRequestMessage request, Request rqFilter)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				int totalRow = 0;
				string filterSlideName = "";
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
				if (rqFilter.filter != null)
				{
					filterSlideName = rqFilter.filter.filters[0].Value;
				}
				var lstSlide = _commomService.GetAll(filterSlideName);
				var lstSlideVm = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(lstSlide);
				totalRow = lstSlideVm.Count();
				var query = lstSlideVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<SlideViewModel>()
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
		public HttpResponseMessage Create(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;
				var slideJson = HttpContext.Current.Request.Unvalidated.Form["slide"];
				if (slideJson == null)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
					return response;
				}
				var SlideVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<SlideViewModel>(slideJson);
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}

				Slide slideDb = new Slide();
				slideDb.UpdateSlide(SlideVm);

				var files = System.Web.HttpContext.Current.Request.Files;				
				if (files.Count != 0)
				{
					var file = files[0];
					Guid id = Guid.NewGuid();
					string extentsion = new FileInfo(file.FileName).Extension.ToLower();
					string fileName = id + "-" + new FileInfo(file.FileName).Name;
					file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
					+ CommonConstants.FolderSlide + "/" + fileName));
					slideDb.Image = fileName;
				}

				_commomService.Add(slideDb);
				_commomService.Save();
				var responseData = Mapper.Map<Slide, SlideViewModel>(slideDb);
				response = request.CreateResponse(HttpStatusCode.OK, responseData);


				return response;
			});
		}

		[Route("show")]
		[HttpPost]
		public HttpResponseMessage Show(HttpRequestMessage request, JObject jsonData)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				dynamic json = jsonData;
				int slideID = json.slideID;
				if (slideID == 0)
				{
					return request.CreateResponse(HttpStatusCode.NoContent);
				}
				var slideDb = _commomService.GetByIdSlide(slideID);
				slideDb.Status = true;

				_commomService.Update(slideDb);
				_commomService.Save();

				response = request.CreateResponse(HttpStatusCode.OK, true);
				return response;
			});
		}

		[Route("unshow")]
		[HttpPost]
		public HttpResponseMessage UnShow(HttpRequestMessage request, JObject jsonData)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				dynamic json = jsonData;
				int slideID = json.slideID;
				if (slideID == 0)
				{
					return request.CreateResponse(HttpStatusCode.NoContent);
				}
				var slideDb = _commomService.GetByIdSlide(slideID);
				slideDb.Status = false;

				_commomService.Update(slideDb);
				_commomService.Save();

				response = request.CreateResponse(HttpStatusCode.OK, true);
				return response;
			});
		}

		[Route("getbyid")]
		[HttpGet]
		public HttpResponseMessage GetID(HttpRequestMessage request, int slideID)
		{
			return CreateHttpResponse(request, () =>
			{
				var slide = _commomService.GetByIdSlide(slideID);
				var slideVm = Mapper.Map<Slide, SlideViewModel>(slide);
				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, slideVm);
				return response;
			});
		}

		[Route("edit")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;


				var slideJson = HttpContext.Current.Request.Unvalidated.Form["slide"];
				if (slideJson == null)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
					return response;
				}
				var SlideVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<SlideViewModel>(slideJson);
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}

				Slide slideDB = new Slide();
				slideDB.UpdateSlide(SlideVm);
				var files = System.Web.HttpContext.Current.Request.Files;
				if (files.Count != 0)
				{
					var file = files[0];
					Guid id = Guid.NewGuid();
					string extentsion = new FileInfo(file.FileName).Extension.ToLower();
					string fileName = id + "-" + new FileInfo(file.FileName).Name;
					file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
					+ CommonConstants.FolderSlide + "/" + fileName));
					slideDB.Image = fileName;
				}

				_commomService.Update(slideDB);
				_commomService.Save();

				var responseData = Mapper.Map<Slide, SlideViewModel>(slideDB);
				response = request.CreateResponse(HttpStatusCode.Created, responseData);
				return response;
			});
		}

		[Route("delete")]
		[HttpPost]
		public HttpResponseMessage Delete(HttpRequestMessage request, JObject jsonData)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;
				if (!ModelState.IsValid)
				{
					request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
				}
				else
				{
					dynamic json = jsonData;
					int slideID = json.slideID;

					_commomService.Delete(slideID);
					_commomService.Save();

					response = request.CreateResponse(HttpStatusCode.Created, true);
				}
				return response;
			});
		}
	}
}
