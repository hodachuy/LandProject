using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Hubs;
using LandProject.Web.Infrastructure.Extensions;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LandProject.Web.Controllers
{
    public class PostingNews2Controller : Controller
    {
        private ILandNewsService _landNewsService;
        private IAgentService _agentService;
        private ILandFileService _landFileService;
        private INotifyService _notifyService;
        private IAddressCommonService _addressCommonService;
        public PostingNews2Controller(ILandNewsService landNewsService,
                                     IAgentService agentService,
                                     ILandFileService landFileService,
                                     INotifyService notifyService,
                                     IAddressCommonService addressCommonService)
        {
            _landNewsService = landNewsService;
            _agentService = agentService;
            _landFileService = landFileService;
            _notifyService = notifyService;
            _addressCommonService = addressCommonService;

        }
        // GET: PostingNews2
        public ActionResult Index()
        {
            if(Session[CommonConstants.SessionUser] == null)
            {
                return RedirectToAction("Login", "Account",new {returnUrl = "/dang-tin-mua-ban-nha-dat.html" });
            }
            return View();
        }
    }
}