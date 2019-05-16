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
	[RoutePrefix("api/lprojectcategory")]
	public class LProjectCategoryController : ApiControllerBase
	{
		private ILProjectCategoryService _lProjectCategoryService;
		public LProjectCategoryController(IErrorService errorService, ILProjectCategoryService lProjectCategoryService) : base(errorService)
		{
			_lProjectCategoryService = lProjectCategoryService;
		}

		[Route("getall")]
		[HttpPost]
		public HttpResponseMessage GetAll(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response;
				var lstLProjectCategory = _lProjectCategoryService.GetAll();
				response = request.CreateResponse(HttpStatusCode.OK, lstLProjectCategory);
				return response;
			});
		}
	}
}
