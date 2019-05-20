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
    [RoutePrefix("api/landtype")]
    public class LandTypeController : ApiControllerBase
    {
        private ILandTypeService _landTypeService;
        public LandTypeController(IErrorService errorService, ILandTypeService landTypeService) : base(errorService)
        {
            _landTypeService = landTypeService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;            
                var lstLandType = _landTypeService.GetAll();
                var lstLandTypeVm = Mapper.Map<IEnumerable<LandType>, IEnumerable<LandTypeViewModel>>(lstLandType);
                response = request.CreateResponse(HttpStatusCode.OK, lstLandTypeVm);
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
                var lstLandType = _landTypeService.GetAllByCondition(filterLTypeName);
                var lstLandTypeVm = Mapper.Map<IEnumerable<LandType>, IEnumerable<LandTypeViewModel>>(lstLandType);
                totalRow = lstLandTypeVm.Count();
                var query = lstLandTypeVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LandTypeViewModel>()
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
		public HttpResponseMessage GetByID(HttpRequestMessage request, int lTypeId)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if(lTypeId == 0)
				{
					return request.CreateResponse(HttpStatusCode.NoContent);
				}
				var landType = _landTypeService.GetByID(lTypeId);
                var landTypeVm = Mapper.Map<LandType, LandTypeViewModel>(landType);
                response = request.CreateResponse(HttpStatusCode.OK, landTypeVm);
				return response;
			});
		}


		[Route("create")]
		[HttpPost]
		public HttpResponseMessage Create(HttpRequestMessage request, LandTypeViewModel lTypeVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LandType landTypeDb = new LandType();
				landTypeDb.UpdateLandType(lTypeVm);
				_landTypeService.Add(landTypeDb);
				_landTypeService.Save();
                var landTypeVm = Mapper.Map<LandType, LandTypeViewModel>(landTypeDb);
                response = request.CreateResponse(HttpStatusCode.OK, landTypeVm);
				return response;
			});
		}

		[Route("update")]
		[HttpPost]
		public HttpResponseMessage Update(HttpRequestMessage request, LandTypeViewModel lTypeVm)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				if (!ModelState.IsValid)
				{
					response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
					return response;
				}
				LandType landTypeDb = new LandType();
				landTypeDb.UpdateLandType(lTypeVm);
				_landTypeService.Update(landTypeDb);
				_landTypeService.Save();
                var landTypeVm = Mapper.Map<LandType, LandTypeViewModel>(landTypeDb);
                response = request.CreateResponse(HttpStatusCode.OK, landTypeVm);
				return response;
			});
		}
	}
}
