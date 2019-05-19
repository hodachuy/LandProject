using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class LProjectCategoryViewModel
	{
		public int ID { set; get; }

		[MaxLength(200)]
		[Required]
		public string Name { set; get; }

		[Required]
		[MaxLength(256)]
		[Column(TypeName = "varchar")]
		public string Alias { set; get; }

		[MaxLength(250)]
		public string Description { set; get; }

		public bool IsDelete { set; get; }

		public bool IsPublished { set; get; }

		public virtual IEnumerable<LProjectViewModel> LProjects { set; get; }
	}
}