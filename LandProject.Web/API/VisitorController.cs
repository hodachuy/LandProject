using LandProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;
using LandProject.Web.Models;
using Newtonsoft.Json.Linq;
using LandProject.Model.Models;
using LandProject.Common;

namespace LandProject.Web.API
{
    [RoutePrefix("api/visitor")]
    public class VisitorController : ApiControllerBase
    {
        private IVisitorService _visitorService;
        public VisitorController(IErrorService errorService, IVisitorService visitorService) : base(errorService)
        {
            _visitorService = visitorService;
        }

        [Route("getstatistic")]
        [HttpPost]
        public HttpResponseMessage GetVisitorStatistic(HttpRequestMessage request, FilterDate filterDate)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                int totalRow = 0;
                string filter = null;
                string sort = null;
                int page = 0;
                int pageSize = 31;

                if (String.IsNullOrEmpty(filterDate.StartDate) && !String.IsNullOrEmpty(filterDate.EndDate))
                {
                    filter = "VisitedDate <= " + "'" + filterDate.EndDate + " 23:59:59" + "'";
                }
                else if (String.IsNullOrEmpty(filterDate.EndDate) && !String.IsNullOrEmpty(filterDate.StartDate))
                {
                    filter += "VisitedDate >= " + "'" + filterDate.StartDate + " 00:00:00" + "'";
                }
                else if (!String.IsNullOrEmpty(filterDate.EndDate) && !String.IsNullOrEmpty(filterDate.StartDate))
                {
                    filter += "VisitedDate >= " + "'" + filterDate.StartDate + " 00:00:00" + "'" + " AND VisitedDate <= " + "'" + filterDate.EndDate + " 23:59:59" + "'";
                }

                var lstVisitorStatistic = _visitorService.VisitorStatisticAccessPage(filter, sort, page, pageSize).ToList();
                if (lstVisitorStatistic.Count() != 0)
                {
                    totalRow = lstVisitorStatistic.Count();
                }
                var paginationSet = new PaginationSet<VisitorStatisticViewModel>()
                {
                    Items = lstVisitorStatistic,
                    Page = page,
                    TotalCount = totalRow,
                    MaxPage = pageSize,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, JObject jsonData)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;
                dynamic json = jsonData;
                string IpAddress = json.IpAddress;
                if (!String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = System.Web.HttpUtility.HtmlDecode(IpAddress);
                }
                bool isHasIPv4InDay = _visitorService.CheckIpAddressExistInDay(IpAddress);
                if (isHasIPv4InDay)
                {
                    response = request.CreateResponse(HttpStatusCode.OK, false);
                    return response;
                }
                VisitorStatistic vsDb = new VisitorStatistic();
                vsDb.ID = Guid.NewGuid();
                vsDb.IPAddress = IpAddress;
                vsDb.VisitedDate = DateTime.Now;
                _visitorService.Add(vsDb);
                _visitorService.Save();
                response = request.CreateResponse(HttpStatusCode.OK, true);
                return response;
            });
        }

        public class FilterDate
        {
            public string StartDate { set; get; }

            public string EndDate { set; get; }
        }
    }
}
