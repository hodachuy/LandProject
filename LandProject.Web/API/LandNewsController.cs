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
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json.Linq;

namespace LandProject.Web.API
{
    [RoutePrefix("api/landnews")]
    public class LandNewsController : ApiControllerBase
    {
        private ILandNewsService _landNewsService;
        private IAgentService _agentService;
        private ILandFileService _landFileService;
        public LandNewsController(IErrorService errorService,
                                    ILandNewsService landNewsService,
                                    IAgentService agentService,
                                    ILandFileService landFileService) : base(errorService)
        {
            _landNewsService = landNewsService;
            _agentService = agentService;
            _landFileService = landFileService;
        }

        [Route("getalltable")]
        [HttpPost]
        public HttpResponseMessage GetAllToTable(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                var formRequest= HttpContext.Current.Request.Unvalidated.Form["requestFilter"];
                var requestFilter = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<Request>(formRequest);
                var typeExchange = HttpContext.Current.Request.Unvalidated.Form["typeExchange"];
                var typeExchangeValue = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<int>(typeExchange);

                int totalRow = 0;
                string filter = Common.CommonSer.ConvertFilertToString(requestFilter.filter);
                string sort = Common.CommonSer.ConvertSortToString(requestFilter.sort);
                int page = requestFilter.page;
                int pageSize = requestFilter.pageSize;

                filter += "lt.TypeExchange = " + typeExchangeValue;
                var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
                if (lstLandNews.Count() != 0)
                {
                    totalRow = lstLandNews[0].Total;
                }
                var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
                {
                    Items = lstLandNews,
                    Page = page,
                    TotalCount = totalRow,
                    MaxPage = pageSize,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getalltablebyaccount")]
        [HttpPost]
        public HttpResponseMessage GetAllToTableByAccount(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                var formRequest = HttpContext.Current.Request.Unvalidated.Form["requestFilter"];
                var requestFilter = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<Request>(formRequest);
                var userID = HttpContext.Current.Request.Unvalidated.Form["userID"];
                var userIDValue = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<string>(userID);

                int totalRow = 0;
                string filter = Common.CommonSer.ConvertFilertToString(requestFilter.filter);
                string sort = Common.CommonSer.ConvertSortToString(requestFilter.sort);
                int page = requestFilter.page;
                int pageSize = requestFilter.pageSize;


                filter += "ln.UserID like '%" + userIDValue + "%'";

                var lstLandNews = _landNewsService.GetAllByFilter(filter, sort, page, pageSize).ToList();
                if (lstLandNews.Count() != 0)
                {
                    totalRow = lstLandNews[0].Total;
                }
                var paginationSet = new PaginationSet<LandNewsFilterViewModel>()
                {
                    Items = lstLandNews,
                    Page = page,
                    TotalCount = totalRow,
                    MaxPage = pageSize,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getbyid")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int landNewsID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (landNewsID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }

                string filter = "ln.ID = " + landNewsID;

                var landNewsDetail = _landNewsService.GetAllByFilter(filter,null, 1, 1).ToList()[0];
                var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNewsDetail);

                var lstLandFile = _landFileService.GetByLandNewsID(landNewsID);
                var lstLandFileVm = Mapper.Map<IEnumerable<LandFile>,IEnumerable<LandFileViewModel>>(lstLandFile);
                landNewsVm.LandFiles = lstLandFileVm;

                var agent = _agentService.GetByID(landNewsVm.AgentID);
                var agentDb = Mapper.Map<Agent, AgentViewModel>(agent);
                landNewsVm.Agent = agentDb;

                response = request.CreateResponse(HttpStatusCode.OK, landNewsVm);
                return response;
            });
        }

        [Route("publishedlandnewsschedule")]
        [HttpPost]
        public HttpResponseMessage PublishedLandnewsSchedule(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                dynamic json = jsonData;
                int landNewsID = json.landNewsID;
                int landNewsScheduleID = json.landScheduleID;
                if (landNewsID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var landNews = _landNewsService.GetLandNewsDbModelByID(landNewsID);

                landNews.LandNewsScheduleID = landNewsScheduleID;
                landNews.PublishedDate = DateTime.Now;
                _landNewsService.PublishedLandnewsSchedule(landNews);
                _landNewsService.Save();

                response = request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            });
        }

        [Route("published")]
        [HttpPost]
        public HttpResponseMessage PublishedLandNews(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                dynamic json = jsonData;
                int landNewsID = json.landNewsID;
                if (landNewsID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var landNews = _landNewsService.GetLandNewsDbModelByID(landNewsID);
                landNews.IsPublished = true;
				landNews.PublishedDate = DateTime.Now;
                _landNewsService.PublishedLandNews(landNews);
                _landNewsService.Save();
                //var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

                response = request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            });
        }

        [Route("canclepublished")]
        [HttpPost]
        public HttpResponseMessage CanclePublishedLandNews(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                dynamic json = jsonData;
                int landNewsID = json.landNewsID;
                if (landNewsID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var landNews = _landNewsService.GetLandNewsDbModelByID(landNewsID);
                landNews.IsPublished = false;
                _landNewsService.PublishedLandNews(landNews);
                _landNewsService.Save();
                //var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

                response = request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            });
        }

        [Route("delete")]
        [HttpPost]
        public HttpResponseMessage Delete(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                dynamic json = jsonData;
                int landNewsID = json.landNewsID;
                if (landNewsID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var landNews = _landNewsService.GetLandNewsDbModelByID(landNewsID);
                landNews.IsDelete = true;
                _landNewsService.DeleteLandNews(landNews);
                _landNewsService.Save();
                //var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

                response = request.CreateResponse(HttpStatusCode.OK, true);
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

                // thông tin nhà đất
                var landNewsJson = HttpContext.Current.Request.Unvalidated.Form["landnews"];
                if (landNewsJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var landNewsVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<LandNewsViewModel>(landNewsJson);
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }

                // thông tin liên hệ
                var agentJson = HttpContext.Current.Request.Unvalidated.Form["agent"];
                if (agentJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var agentVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<AgentViewModel>(agentJson);

                // save agent
                Agent agentDb = new Agent();
                agentDb.UpdateAgent(agentVm);
                _agentService.Add(agentDb);
                _agentService.Save();

                // save landnews
                LandNews landNewsDb = new LandNews();
                landNewsDb.AgentID = agentDb.ID;
                landNewsDb.Code = DateTime.Now.ToString() + agentDb.ID;
                landNewsDb.UpdateLandNews(landNewsVm);

                // image avatar
                var files = HttpContext.Current.Request.Files;
                if(files.Count != 0)
                {
                    var file = files[0];
                    Guid id = Guid.NewGuid();
                    string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                    string fileName = id + "-" + new FileInfo(file.FileName).Name;
                    file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                    + CommonConstants.FolderLandNews + "/" + fileName));
                    landNewsDb.Image = fileName;
                }

                _landNewsService.Add(landNewsDb);
                _landNewsService.Save();

                // save landfile
                if(files.Count != 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        Guid id = Guid.NewGuid();
                        string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                        string fileName = id + "-" + new FileInfo(file.FileName).Name;
                        file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                        + CommonConstants.FolderLandNews + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            LandNewsID = landNewsDb.ID
                        };
                        _landFileService.Add(fileImage);
                    }
                    _landFileService.Save();
                }

                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    code = landNewsDb.Code,// return mã tin nhắn của bạn
                    status = true
                });
                return response;
            });
        }

    }
}