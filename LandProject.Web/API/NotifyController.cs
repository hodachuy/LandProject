using AutoMapper;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LandProject.Web.API
{

	[RoutePrefix("api/notify")]
	public class NotifyController : ApiControllerBase
	{
		INotifyService _notifyService;
		public NotifyController(IErrorService errorService, INotifyService notifyService) : base(errorService)
		{
			_notifyService = notifyService;
		}

		[Route("getall")]
		[HttpGet]
		public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize)
		{

			return CreateHttpResponse(request, () =>
			{
				int totalRow = 0;

				var listNotify = _notifyService.GetAll();

				totalRow = listNotify.Count();

				// lay so ban ghi su dung de phan trang tu du lieu cua model.
				var query = listNotify.Skip(page * pageSize).Take(pageSize).OrderByDescending(x => x.CreatedDate);
				var lstNotifyVm = Mapper.Map<IEnumerable<Notify>, IEnumerable<NotifyViewModel>>(query);

				// Khai bao paginationSet de the hien du lieu json tren web api voi cac thong tin ve page,totalcount,totalpage,ienumerable<model>
				var paginationSet = new PaginationSet<NotifyViewModel>()
				{
					Items = lstNotifyVm,
					Page = page,
					TotalCount = totalRow,
					TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
				};

				// hien thi du lieu pageinationSet json
				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

				return response;
			});
		}

		[Route("getnoread")]
		[HttpGet]
		public HttpResponseMessage GetMessageNotRead(HttpRequestMessage request)
		{

			return CreateHttpResponse(request, () =>
			{
				int totalRow = 0;
				string table = "LandNews";
				var listNotifyNoRead = _notifyService.GetNewMessage(table).Where(x=>x.IsRead == false);
				var lstNotifyVm = Mapper.Map<IEnumerable<Notify>, IEnumerable<NotifyViewModel>>(listNotifyNoRead);
				totalRow = lstNotifyVm.Count();
				var paginationSet = new PaginationSet<NotifyViewModel>()
				{
					Items = lstNotifyVm,
					Page = 1,
					TotalCount = totalRow,
					TotalPages = 1
				};
				HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
				return response;
			});
		}
		[Route("updateall")]
		[HttpGet]
		public HttpResponseMessage Update(HttpRequestMessage request)
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
					var listNotify = _notifyService.GetNewMessage("LandNews").Where(x => x.IsRead == false);
					var lstNotifyVm = Mapper.Map<IEnumerable<Notify>, IEnumerable<NotifyViewModel>>(listNotify);
					if(lstNotifyVm.Count() != 0)
					{
						foreach(var item in lstNotifyVm)
						{
							var notifyDb = _notifyService.GetById(item.ID);
							notifyDb.IsRead = true;
							_notifyService.Update(notifyDb);
							_notifyService.Save();
						}
					}				
					response = request.CreateResponse(HttpStatusCode.Created);
				}

				return response;
			});
		}
	}
}