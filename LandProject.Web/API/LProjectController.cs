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
using AutoMapper;
using LandProject.Web.Models;
using System.Web;
using System.Web.Script.Serialization;
using LandProject.Web.Infrastructure.Extensions;
using System.IO;

namespace LandProject.Web.API
{
    [RoutePrefix("api/lproject")]
    public class LProjectController : ApiControllerBase
    {
        private ILProjectService _lProjectService;
        private ILandFileService _landFileService;
        public LProjectController(IErrorService errorService,
            ILProjectService lProjectService,
            ILandFileService landFileService) : base(errorService)
        {
            _lProjectService = lProjectService;
            _landFileService = landFileService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                var lstLProject = _lProjectService.GetAll();
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
                return response;
            });
        }

        [Route("getbylproject")]
        [HttpGet]
        public HttpResponseMessage GetAllByLProjectCategory(HttpRequestMessage request, int lProjectCategoryID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (lProjectCategoryID == 0)
                    return request.CreateResponse(HttpStatusCode.NoContent);

                var lstLProject = _lProjectService.GetByLProjectCategoryID(lProjectCategoryID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
                return response;
            });
        }

        [Route("getbydistrict")]
        [HttpGet]
        public HttpResponseMessage GetAllByDistrict(HttpRequestMessage request, int districtID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (districtID == 0)
                    return request.CreateResponse(HttpStatusCode.NoContent);

                var lstLProject = _lProjectService.GetByDistrictID(districtID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                response = request.CreateResponse(HttpStatusCode.OK, lstLProjectVm);
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
                string filterLProjectName = "";
                int filterLTypeID = 0;
                rqFilter.pageSize = rqFilter.pageSize == 0 ? 20 : rqFilter.pageSize;
                rqFilter.page = (rqFilter.pageSize == 0 ? 1 : rqFilter.page) - 1;
                if (rqFilter.filter != null)
                {
                    foreach (var item in rqFilter.filter.filters)
                    {
                        if (item.Field == "LProjectCategoryID")
                        {
                            filterLTypeID = Int32.Parse(item.Value);
                        }
                        if (item.Field == "LProjectName")
                        {
                            filterLProjectName = item.Value;
                        }
                    }
                }
                var lstLProject = _lProjectService.GetAllByCondition(filterLProjectName, filterLTypeID);
                var lstLProjectVm = Mapper.Map<IEnumerable<LProject>, IEnumerable<LProjectViewModel>>(lstLProject);
                totalRow = lstLProjectVm.Count();
                var query = lstLProjectVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<LProjectViewModel>()
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
        public HttpResponseMessage GetByID(HttpRequestMessage request, int lProjectID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (lProjectID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var lprjCategory = _lProjectService.GetByID(lProjectID);
                var lprjCategoryVm = Mapper.Map<LProject, LProjectViewModel>(lprjCategory);
                response = request.CreateResponse(HttpStatusCode.OK, lprjCategoryVm);
                return response;
            });
        }


        [Route("getfilelproject")]
        [HttpGet]
        public HttpResponseMessage GetFileLProject(HttpRequestMessage request, int lProjectID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (lProjectID == 0)
                    return request.CreateResponse(HttpStatusCode.NoContent);

                var lstLandFile = _landFileService.GetByLProjectID(lProjectID);
                var lstLandFileVm = Mapper.Map<IEnumerable<LandFile>, IEnumerable<LandFileViewModel>>(lstLandFile);
                response = request.CreateResponse(HttpStatusCode.OK, lstLandFileVm);
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

                // thông tin dự án
                var lProjectJson = HttpContext.Current.Request.Unvalidated.Form["lproject"];
                if (lProjectJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var lProjectVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<LProjectViewModel>(lProjectJson);
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }


                var files = System.Web.HttpContext.Current.Request.Files;
                List<LandFile> lstLProjectFile = new List<LandFile>();
                if (files.Count != 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        Guid id = Guid.NewGuid();
                        string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                        string fileName = id + "-" + new FileInfo(file.FileName).Name;
                        file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                        + CommonConstants.FolderLProject + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            LProjectID = 0,
                        };
                        lstLProjectFile.Add(fileImage);
                    }
                }

                // save lProjectDb
                LProject lProjectDb = new LProject();
                lProjectDb.UpdateLProject(lProjectVm);
                if (lstLProjectFile.Count() != 0)
                {
                    lProjectDb.Image = lstLProjectFile[0].FileName;
                }

                _lProjectService.Add(lProjectDb);
                _lProjectService.Save();


                if (lstLProjectFile.Count() != 0)
                {
                    foreach (var item in lstLProjectFile)
                    {
                        item.LProjectID = lProjectDb.ID;
                        _landFileService.Add(item);
                    }
                    _landFileService.Save();
                }
                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    status = true
                });
                return response;
            });
        }



        [Route("update")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                // thông tin dự án
                var lProjectJson = HttpContext.Current.Request.Unvalidated.Form["lproject"];
                if (lProjectJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var lProjectVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<LProjectViewModel>(lProjectJson);
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    return response;
                }


                var files = System.Web.HttpContext.Current.Request.Files;
                List<LandFile> lstLProjectFile = new List<LandFile>();
                if (files.Count != 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        Guid id = Guid.NewGuid();
                        string extentsion = new FileInfo(file.FileName).Extension.ToLower();
                        string fileName = id + "-" + new FileInfo(file.FileName).Name;
                        file.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                        + CommonConstants.FolderLProject + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            LProjectID = lProjectVm.ID,
                        };
                        _landFileService.Add(fileImage);
                    }
                    _landFileService.Save();
                }

                // save lProjectDb
                LProject lProjectDb = new LProject();
                lProjectDb.UpdateLProject(lProjectVm);

                var lstFile = _landFileService.GetByLProjectID(lProjectVm.ID).ToList();

                if (lstFile.Count() == 0)
                {
                    lProjectDb.Image = "";
                }else
                {
                    lProjectDb.Image = lstFile[0].FileName;
                }

                _lProjectService.Update(lProjectDb);
                _lProjectService.Save();

                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    status = true
                });
                return response;
            });
        }
    }
}
