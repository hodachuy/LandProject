using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
	public class LandCategoryViewModel
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

		[Required]
		public int LandTypeID { set; get; }

		public virtual LandTypeViewModel LandType { set; get; }

		public bool IsDelete { set; get; }
	}
}