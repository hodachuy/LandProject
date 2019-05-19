using BotProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LandProject.Service;

namespace LandProject.Web.API
{
	[RoutePrefix("api/address")]
	public class AddressCommonController : ApiControllerBase
	{
		private IAddressCommonService _addressCommonService;
		public AddressCommonController(IErrorService errorService, IAddressCommonService addressCommonService) : base(errorService)
		{
			_addressCommonService = addressCommonService;
		}

		[Route("getprovince")]
		[HttpGet]
		public HttpResponseMessage GetProvince(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstProvince = _addressCommonService.GetAllProvince();
				response = request.CreateResponse(HttpStatusCode.OK, lstProvince);
				return response;
			});
		}

		[Route("getdistrict")]
		[HttpGet]
		public HttpResponseMessage GetDistrict(HttpRequestMessage request, int provinceID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstDistrict = _addressCommonService.GetDistrictByProvinceID(provinceID);
				response = request.CreateResponse(HttpStatusCode.OK, lstDistrict);
				return response;
			});
		}

		[Route("getward")]
		[HttpGet]
		public HttpResponseMessage GetWard(HttpRequestMessage request, int districtID)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstWard = _addressCommonService.GetWardByDistrictID(districtID);
				response = request.CreateResponse(HttpStatusCode.OK, lstWard);
				return response;
			});
		}
	}
}
