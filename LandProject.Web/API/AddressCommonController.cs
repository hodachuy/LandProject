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

	}
}
