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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LandProject.Web.API
{
	[Authorize]
	[RoutePrefix("api/menugroup")]
	public class MenuGroupController : ApiControllerBase
	{
		IMenuGroupService _menuGroupService;
		IMenuService _menuService;
		public MenuGroupController(IErrorService errorService, IMenuGroupService menuGroupService, IMenuService menuService) : base(errorService)
		{
			_menuGroupService = menuGroupService;
			_menuService = menuService;
		}


		/// <summary>
		/// Get all name ProductCategory for method seleclist.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("getall")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				var listMenuGroup = _menuGroupService.GetAll();

				// map chuyen doi du lieu tu model toi du lieu trang view model
				var listMenuGroupVm = Mapper.Map<IEnumerable<MenuGroup>, IEnumerable<MenuGroupViewModel>>(listMenuGroup);
				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, listMenuGroupVm);
				return response;
			});
		}

		[Route("getalltable")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request, Request rqFilter)
		{
			return CreateHttpResponse(request, () => {
				HttpResponseMessage response = null;

				int totalRow = 0;
				string filterLTypeName = "";
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
				if (rqFilter.filter != null)
				{
					filterLTypeName = rqFilter.filter.filters[0].Value;
				}

				var lstMenuGroup = _menuGroupService.GetAll(filterLTypeName);
				var lstMenuGroupVm = Mapper.Map<IEnumerable<MenuGroup>, IEnumerable<MenuGroupViewModel>>(lstMenuGroup);
				totalRow = lstMenuGroupVm.Count();
				var query = lstMenuGroupVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<MenuGroupViewModel>()
				{
					Items = lstMenuGroupVm,
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
		public HttpResponseMessage Create(HttpRequestMessage request, MenuGroupViewModel menuGroupVm)
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

					MenuGroup menuGroupDb = new MenuGroup();

					// Chuyen doi du lieu tu trang view toi du lieu cua model
					menuGroupDb.UpdateMenuGroup(menuGroupVm);


					// sau khi chuyen doi ta add du lieu
					_menuGroupService.Add(menuGroupDb);
					_menuGroupService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<MenuGroup, MenuGroupViewModel>(menuGroupDb);
					response = request.CreateResponse(HttpStatusCode.OK, responseData);
				}

				return response;
			});
		}

		[Route("getbyid")]
		[HttpGet]
		public HttpResponseMessage GetID(HttpRequestMessage request, int menuGroupID)
		{
			return CreateHttpResponse(request, () =>
			{
				var menugroup = _menuGroupService.GetByIdMenuGroup(menuGroupID);

				// Map chuyen doi du lieu tu model toi du lieu trang view model.
				var menuGroupVm = Mapper.Map<MenuGroup, MenuGroupViewModel>(menugroup);

				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, menuGroupVm);

				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, MenuGroupViewModel menuGroupVm)
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
					MenuGroup menuGroupDB = new MenuGroup();

					// Chuyen doi du lieu tu trang view toi du lieu cua model
					menuGroupDB.UpdateMenuGroup(menuGroupVm);

					// sau khi chuyen doi ta add du lieu
					_menuGroupService.Update(menuGroupDB);
					_menuGroupService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<MenuGroup, MenuGroupViewModel>(menuGroupDB);
					response = request.CreateResponse(HttpStatusCode.Created, responseData);
				}

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
					int menuGroupID = json.menuGroupID;

					var lstMenu = _menuService.GetMenuByMenuGroup(menuGroupID);

					foreach (var item in lstMenu)
					{
						_menuService.Delete(item.ID);
					}

					var menuGroup = _menuGroupService.Delete(menuGroupID);
					_menuGroupService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<MenuGroup, MenuGroupViewModel>(menuGroup);
					response = request.CreateResponse(HttpStatusCode.Created, responseData);
				}
				return response;
			});
		}

		[Route("deletemulti")]
		[HttpDelete]
		public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstMenuGroupID)
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
					var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstMenuGroupID);
					foreach (var item in lstItemID)
					{
						var lstMenu = _menuService.GetMenuByMenuGroup(item);

						foreach (var item1 in lstMenu)
						{
							_menuService.Delete(item1.ID);
						}

						_menuGroupService.Delete(item);
					}
					_menuGroupService.Save();

					response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
				}
				return response;
			});
		}

	}
}
