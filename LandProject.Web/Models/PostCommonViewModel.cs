using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class PostCommonViewModel
	{
		public IEnumerable<PostViewModel> PostTop { set; get; }

		public IEnumerable<PostCategoryViewModel> PostListCategory { set; get; }
	}
}