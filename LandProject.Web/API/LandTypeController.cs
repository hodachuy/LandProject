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
    [RoutePrefix("api/landtype")]
    public class LandTypeController : ApiControllerBase
    {
        private ILandTypeService _landTypeService;
        public LandTypeController(IErrorService errorService, ILandTypeService landTypeService) : base(errorService)
        {
            _landTypeService = landTypeService;
        }

        [Route("getall")]
        [HttpPost]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;            
                var lstLandType = _landTypeService.GetAll();
                response = request.CreateResponse(HttpStatusCode.OK, lstLandType);
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
                string filter = "";
                rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
                rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
                if (rqFilter.filter != null)
                {
                    filter = rqFilter.filter.filters[0].Value;
                }
                var lstLandType = _landTypeService.GetAllByCondition(filter);
                totalRow = lstLandType.Count();
                var query = lstLandType.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LandType>()
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
