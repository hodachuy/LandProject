using AutoMapper;
using LandProject.Common;
using LandProject.Model.Models;
using LandProject.Service;
using LandProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LandProject.Web.Controllers
{
    public class LandNewsController : Controller
    {
        private ILandNewsService _landNewsService;
        private ILandFileService _landFileService;
        private IAgentService _agentService;
        public LandNewsController(ILandNewsService landNewsService,
            ILandFileService landFileService,
            IAgentService agentService)
        {
            _landNewsService = landNewsService;
            _landFileService = landFileService;
            _agentService = agentService;
        }
        // GET: LandNews
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var landNews = _landNewsService.GetByID(id);
            var landNewsVm = Mapper.Map<LandNewsFilterViewModel, LandNewsViewModel>(landNews);

            var lstLandFile = _landFileService.GetByLandNewsID(id);
            var lstLandFileVm = Mapper.Map<IEnumerable<LandFile>, IEnumerable<LandFileViewModel>>(lstLandFile);
            landNewsVm.LandFiles = lstLandFileVm;

            var agent = _agentService.GetByID(landNewsVm.AgentID);
            var agentDb = Mapper.Map<Agent, AgentViewModel>(agent);
            landNewsVm.Agent = agentDb;

            return View(landNewsVm);
        }

		public ActionResult LandNewsByLandType(int landTypeID)
		{
			return View();
		}
		public ActionResult LandNewsByLandCategory(int landCategoryID)
		{
			return View();
		}
		public ActionResult LandNewsByWard(int wardID)
		{
			return View();
		}
		public ActionResult LandNewsByDistrict(int districtID)
		{
			return View();
		}
		public ActionResult LandNewsByProvince(int provinceID)
		{
			return View();
		}

		public ActionResult SearchLandNews()
		{
			return View();
		}
	}
}