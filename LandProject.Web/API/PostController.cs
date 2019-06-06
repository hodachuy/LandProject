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
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using LandProject.Web.Infrastructure.Extensions;

namespace LandProject.Web.API
{
    [RoutePrefix("api/post")]
    public class PostController : ApiControllerBase
    {
        IPostService _postService;
        ILandFileService _landFileService;
        public PostController(IErrorService errorService, IPostService postService,
            ILandFileService landFileService) : base(errorService)
        {
            _postService = postService;
            _landFileService = landFileService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                var lstPost = _postService.GetAll();
                var lstPostVm = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(lstPost);
                response = request.CreateResponse(HttpStatusCode.OK, lstPostVm);
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
                var lstPost = _postService.GetAllByCondition(filterLTypeName);
                var lstPostVm = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(lstPost);
                totalRow = lstPostVm.Count();
                var query = lstPostVm.Skip(rqFilter.page * rqFilter.pageSize).Take(rqFilter.pageSize);
                var paginationSet = new PaginationSet<PostViewModel>()
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
        public HttpResponseMessage GetByID(HttpRequestMessage request, int postID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (postID == 0)
                {
                    return request.CreateResponse(HttpStatusCode.NoContent);
                }
                var post = _postService.GetById(postID);
                var postVm = Mapper.Map<Post, PostViewModel>(post);
                response = request.CreateResponse(HttpStatusCode.OK, postVm);
                return response;
            });
        }


        [Route("getfilelpost")]
        [HttpGet]
        public HttpResponseMessage GetFileLProject(HttpRequestMessage request, int postID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                if (postID == 0)
                    return request.CreateResponse(HttpStatusCode.NoContent);

                var lstLandFile = _landFileService.GetByPostID(postID);
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

                var postJson = HttpContext.Current.Request.Unvalidated.Form["post"];
                if (postJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var postVm = new JavaScriptSerializer{ MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<PostViewModel>(postJson);
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
                        + CommonConstants.FolderPost + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            PostID = 0,
                        };
                        lstLProjectFile.Add(fileImage);
                    }
                }

                // save lProjectDb
                Post postDb = new Post();
                postDb.UpdatePost(postVm);
                postDb.CreatedDate = DateTime.Now;
                if (lstLProjectFile.Count() != 0)
                {
                    postDb.Image = lstLProjectFile[0].FileName;
                }

                _postService.Add(postDb);
                _postService.SaveChanges();


                if (lstLProjectFile.Count() != 0)
                {
                    foreach (var item in lstLProjectFile)
                    {
                        item.PostID = postDb.ID;
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
                var postJson = HttpContext.Current.Request.Unvalidated.Form["post"];
                if (postJson == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    return response;
                }
                var postVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<PostViewModel>(postJson);
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
                        + CommonConstants.FolderPost + "/" + fileName));

                        var fileImage = new LandFile()
                        {
                            FileName = fileName,
                            Extension = extentsion,
                            PostID = postVm.ID,
                        };
                        _landFileService.Add(fileImage);
                    }
                    _landFileService.Save();
                }

                // save lProjectDb
                Post postDb = new Post();
                postDb.UpdatePost(postVm);
                postDb.UpdatedDate = DateTime.Now;

                var lstFile = _landFileService.GetByPostID(postDb.ID).ToList();

                if (lstFile.Count() == 0)
                {
                    postDb.Image = "";
                }
                else
                {
                    postDb.Image = lstFile[0].FileName;
                }

                _postService.Update(postDb);
                _postService.SaveChanges();

                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    status = true
                });
                return response;
            });
        }
    }
}
