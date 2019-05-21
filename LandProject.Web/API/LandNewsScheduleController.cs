using LandProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;
using AutoMapper;
using LandProject.Model.Models;
using LandProject.Web.Models;
using LandProject.Common;
using LandProject.Web.Infrastructure.Extensions;

namespace LandProject.Web.API
{
    [RoutePrefix("api/schedule")]
    public class LandNewsScheduleController : ApiControllerBase
    {
        private ILandNewsScheduleService _landNewsScheduleService;
        public LandNewsScheduleController(IErrorService errorService, ILandNewsScheduleService landNewsScheduleService) : base(errorService)
        {
            _landNewsScheduleService = landNewsScheduleService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                var lstLandSchedule = _landNewsScheduleService.GetAll();
                var lstLandScheduleVm = Mapper.Map<IEnumerable<LandNewsSchedule>, IEnumerable<LandNewsScheduleViewModel>>(lstLandSchedule);
                response = request.CreateResponse(HttpStatusCode.OK, lstLandScheduleVm);
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
                var lstscheduleVm = _landNewsScheduleService.GetAllByCondition(filterLTypeName);
                var lstscheduleVmVm = Mapper.Map<IEnumerable<LandNewsSchedule>, IEnumerable<LandNewsScheduleViewModel>>(lstscheduleVm);
                totalRow = lstscheduleVmVm.Count();
                var query = lstscheduleVmVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LandNewsScheduleViewModel>()
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
        public HttpResponseMessage GetByID(HttpRequestMessage request, int scheduleId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (scheduleId == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var scheduleVm = _landNewsScheduleService.GetByID(scheduleId);
                var scheduleVmVm = Mapper.Map<LandNewsSchedule, LandNewsScheduleViewModel>(scheduleVm);
                response = request.CreateResponse(HttpStatusCode.OK, scheduleVmVm);
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, LandNewsScheduleViewModel scheduleVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }
                LandNewsSchedule landScheduleDb = new LandNewsSchedule();
                landScheduleDb.UpdateLandNewsChedule(scheduleVm);
                _landNewsScheduleService.Add(landScheduleDb);
                _landNewsScheduleService.Save();
                var scheduleVmVm = Mapper.Map<LandNewsSchedule, LandNewsScheduleViewModel>(landScheduleDb);
                response = request.CreateResponse(HttpStatusCode.OK, scheduleVmVm);
                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request, LandNewsScheduleViewModel scheduleVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }
                LandNewsSchedule landScheduleDb = new LandNewsSchedule();
                landScheduleDb.UpdateLandNewsChedule(scheduleVm);
                _landNewsScheduleService.Update(landScheduleDb);
                _landNewsScheduleService.Save();
                var scheduleVmVm = Mapper.Map<LandNewsSchedule, LandNewsScheduleViewModel>(landScheduleDb);
                response = request.CreateResponse(HttpStatusCode.OK, scheduleVmVm);
                return response;
            });
        }
    }
}
