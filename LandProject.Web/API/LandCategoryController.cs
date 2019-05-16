using BotProject.Web.Infrastructure.Core;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LandProject.Web.API
{
    [RoutePrefix("api/landcategory")]
    public class LandCategoryController : ApiControllerBase
    {
        private ILandCategoryService _landCategoryService;
        public LandCategoryController(IErrorService errorService, ILandCategoryService landCategoryService) : base(errorService)
        {
            _landCategoryService = landCategoryService;
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
                int lTypeID = 0;
                rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
                rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
                if (rqFilter.filter != null)
                {
                    foreach(var item in rqFilter.filter.filters)
                    {
                        if(item.Field == "LandTypeID")
                        {
                            lTypeID = Int32.Parse(item.Value);
                        }
                        if(item.Field == "Name")
                        {
                            filter = item.Value;
                        }
                    }
                }
                var lstLandType = _landCategoryService.GetAllByCondition(filter, lTypeID);
                totalRow = lstLandType.Count();
                var query = lstLandType.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LandCategory>()
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
