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
                var lstProvinceVm = Mapper.Map<IEnumerable<Province>, IEnumerable<ProvinceViewModel>>(lstProvince);
                response = request.CreateResponse(HttpStatusCode.OK, lstProvinceVm);
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
                var lstDistrictVm = Mapper.Map<IEnumerable<District>, IEnumerable<DistrictViewModel>>(lstDistrict);
                response = request.CreateResponse(HttpStatusCode.OK, lstDistrictVm);
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
                var lstWardVm = Mapper.Map<IEnumerable<Ward>, IEnumerable<WardViewModel>>(lstWard);
                response = request.CreateResponse(HttpStatusCode.OK, lstWardVm);
				return response;
			});
		}
	}
}
