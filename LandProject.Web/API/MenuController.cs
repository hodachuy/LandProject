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
	[RoutePrefix("api/menu")]
	public class MenuController : ApiControllerBase
    {
		#region
		IMenuService _menuService;
		IMenuGroupService _menuGroupService;

		public MenuController(IErrorService errorService, IMenuService menuService, IMenuGroupService menuGroupService) : base(errorService)
        {
			_menuService = menuService;
			_menuGroupService = menuGroupService;
		}

		#endregion

		/// <summary>
		/// Get all name ProductCategory for method seleclist.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("getparent")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				var listMenu = _menuService.GetAll();

				// map chuyen doi du lieu tu model toi du lieu trang view model
				var menuVm = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(listMenu);
				dynamic model = menuVm;
				// hien thi du lieu pageinationSet json
				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, new { model });

				return response;
			});
		}

		[Route("getalltable")]
		[HttpPost]
		public HttpResponseMessage GetAll(HttpRequestMessage request, Request rqFilter)
		{

			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				int totalRow = 0;
				string filterMenuName = "";
				int filterMenuGroupID = 0;
				rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
				rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;

				if (rqFilter.filter != null)
				{
					foreach (var item in rqFilter.filter.filters)
					{
						if (item.Field == "MenuGroupID")
						{
							filterMenuGroupID = Int32.Parse(item.Value);
						}
						if (item.Field == "Name")
						{
							filterMenuName = item.Value;
						}
					}
				}

				var lstMenu = _menuService.GetAll(filterMenuGroupID, filterMenuName);
				var lstMenuVm = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(lstMenu);
				foreach (var item in lstMenuVm)
				{
					item.NameMenuGroup = _menuGroupService.GetByIdMenuGroup(item.MenuGroupID).Name;
				}
				totalRow = lstMenuVm.Count();
				var query = lstMenuVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
				var paginationSet = new PaginationSet<MenuViewModel>()
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


		/// <summary>
		/// Get data with infor id of ProductCategory.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="productCategoryID"> infor id </param>
		/// <returns></returns>
		[Route("getbyid")]
		[HttpGet]
		public HttpResponseMessage GetID(HttpRequestMessage request, int menuID)
		{
			return CreateHttpResponse(request, () =>
			{
				var menu = _menuService.GetByIdMenu(menuID);
				//if (!String.IsNullOrEmpty(menu.Image))
				//{
				//	// Convert image to base 64 string.
				//	menu.Image = ConvertData.ImageToBase64String(menu.Image, CommonConstants.PathMenu);
				//}


				// Map chuyen doi du lieu tu model toi du lieu trang view model.
				var menuVm = Mapper.Map<Menu, MenuViewModel>(menu);

				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, menuVm);

				return response;
			});
		}

		[Route("create")]
		[HttpPost]
		public HttpResponseMessage Create(HttpRequestMessage request, MenuViewModel menuVm)
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

					Menu menu = new Menu();

					// Chuyen doi du lieu tu trang view toi du lieu cua model
					menu.UpdateMenu(menuVm);

					//if (!String.IsNullOrEmpty(menuVm.Image))
					//{
					//	var image = ConvertData.Base64ToImage(menuVm.Image);
     //                   string extension = HelperMethods.ImageType(image);
     //                   string time = DateTime.Now.ToString("ddMMyyyy");
     //                   string nameImage = time + "_" + CommonConstants.PathMenu + "_" + menu.Alias + extension;
     //                   menu.Image = nameImage;

     //                   string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
					//		+ CommonConstants.PathMenu + "/" + menu.Image);


     //                   HelperMethods.SaveImageByFormat(image, filePath);
     //               }


					// sau khi chuyen doi ta add du lieu
					_menuService.Add(menu);
					_menuService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<Menu, MenuViewModel>(menu);
					response = request.CreateResponse(HttpStatusCode.OK, responseData);
				}

				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, MenuViewModel menuVm)
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
					Menu menu = new Menu();

					// Chuyen doi du lieu tu trang view toi du lieu cua model
					menu.UpdateMenu(menuVm);


					//if (!String.IsNullOrEmpty(menuVm.Image))
					//{
					//	var image = ConvertData.Base64ToImage(menuVm.Image);
					//	string extension = HelperMethods.ImageType(image);
					//	string time = DateTime.Now.ToString("ddMMyyyy");
     //                   string nameImage = time + "_" + CommonConstants.PathMenu + "_" + menu.Alias + extension;
     //                   menu.Image = nameImage;

     //                   string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
					//		+ CommonConstants.PathMenu + "\\" + menu.Image);

     //                   HelperMethods.SaveImageByFormat(image, filePath);
					//}

					//File.WriteAllBytes(string path,byte[] bytes);


					// sau khi chuyen doi ta add du lieu
					_menuService.Update(menu);
					_menuService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<Menu, MenuViewModel>(menu);
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
					int menuID = json.menuID;
					var menu = _menuService.Delete(menuID);
					_menuService.Save();

					//Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
					var responseData = Mapper.Map<Menu, MenuViewModel>(menu);
					response = request.CreateResponse(HttpStatusCode.Created, responseData);
				}
				return response;
			});
		}

		[Route("deletemulti")]
		[HttpDelete]
		public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstMenuID)
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
					var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstMenuID);
					foreach (var item in lstItemID)
					{
						_menuService.Delete(item);
					}
					_menuService.Save();

					response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
				}
				return response;
			});
		}
	}
}
