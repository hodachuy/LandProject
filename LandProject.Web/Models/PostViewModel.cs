using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class PostViewModel
	{
		public int ID { set; get; }

		[Required]
		[MaxLength(256)]
		public string Name { set; get; }

		public string Alias { set; get; }

		[Required]
		public int PostCategoryID { set; get; }

		[MaxLength(256)]
		public string Image { set; get; }

		[MaxLength(500)]
		public string Description { set; get; }

		public string Content { set; get; }
		public DateTime? CreatedDate { set; get; }

		public string CreatedBy { set; get; }

		public DateTime? UpdatedDate { set; get; }

        public bool Status { set; get; }

		public string UpdatedBy { set; get; }

		public string MetaKeyword { set; get; }

		public string MetaDescription { set; get; }

		public bool IsPublished { set; get; }
		public bool IsDelete { set; get; }
		public int? ViewCount { set; get; }
		public virtual PostCategoryViewModel PostCategory { set; get; }

	}
}