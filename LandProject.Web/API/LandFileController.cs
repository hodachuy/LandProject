using LandProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;
using Newtonsoft.Json.Linq;

namespace LandProject.Web.API
{
    [RoutePrefix("api/file")]
    public class LandFileController : ApiControllerBase
    {
        private ILandFileService _landFileService;
        public LandFileController(IErrorService errorService, ILandFileService landFileService) : base(errorService)
        {
            _landFileService = landFileService;
        }

        [Route("delete")]
        [HttpPost]
        public HttpResponseMessage GetFileLProject(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                dynamic json = jsonData;
                int fileID = json.fileID;
                _landFileService.Delete(fileID);
                _landFileService.Save();
                response = request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            });
        }

    }
}
