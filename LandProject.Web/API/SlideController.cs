using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Infrastructure.Core;
using LandProject.Web.Infrastructure.Extensions;
using LandProject.Web.Models;
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
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, Request rqFilter)
        {
            return CreateHttpResponse(request, () => {
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
        public HttpResponseMessage Create(HttpRequestMessage request, SlideViewModel SlideVm)
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
                    Slide slideDb = new Slide();
                    // Chuyen doi du lieu tu trang view toi du lieu cua model
                    slideDb.UpdateSlide(SlideVm);                 
                    // sau khi chuyen doi ta add du lieu
                    _commomService.Add(slideDb);
                    _commomService.Save();
                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slideDb);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }

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
        public HttpResponseMessage Update(HttpRequestMessage request, SlideViewModel slideVm)
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
                    Slide slideDB = new Slide();

                    // Chuyen doi du lieu tu trang view toi du lieu cua model
                    slideDB.UpdateSlide(slideVm);

                    // sau khi chuyen doi ta add du lieu
                    _commomService.Update(slideDB);
                    _commomService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slideDB);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpPost]
        public HttpResponseMessage Delete(HttpRequestMessage request, int slideID)
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
                    var slide = _commomService.Delete(slideID);
                    _commomService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slide);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }      
    }
}
