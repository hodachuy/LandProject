using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class LandNewsDetailViewModel
	{
		public LandNewsViewModel LandNewsDetail { set; get; }
		public IEnumerable<LandNewsViewModel> LandNewsRelated { set; get; }
	}
}