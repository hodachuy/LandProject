using LandProject.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
	[Table("LProjects")]
	public class LProject:Auditable
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { set; get; }

		[MaxLength(200)]
		[Required]
		public string Name { set; get; }

		[Required]
		[MaxLength(256)]
		[Column(TypeName = "varchar")]
		public string Alias { set; get; }

		[StringLength(256)]
		public string Address { set; get; }

        public string Image { set; get; }

		public string Description { set; get; }

		public string Detail { set; get; }

		public string Investors { set; get; }

		[Required]
		public int LProjectCaregoryID { set; get; }

		[ForeignKey("LProjectCaregoryID")]
		public virtual LProjectCategory LProjectCategory { set; get; }

		public decimal? Price { set; get; }

        public string TotalPrice { set; get; }

        public decimal? DecimalTotalPrice { set; get; }

        public string Unit { set; get; }

		public string Area { set; get; }

		public string Size { set; get; }

		[Required]
		public int ProvinceID { set; get; }

		[Required]

		public int DistrictID { set; get; }

		public int WardID { set; get; }

		public bool IsDelete { set; get; }

		public bool IsPublished { set; get; }
	}
}
