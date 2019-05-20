using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
	[Table("Wards")]
	public class Ward
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { set; get; }

		[MaxLength(250)]
		[Required]
		public string Name { set; get; }

		[MaxLength(50)]
		public string Type { set; get; }

		[MaxLength(50)]
		public string LatiLongTude { set; get; }

		[Required]
		public int DistrictID { set; get; }

        [ForeignKey("DistrictID")]
        public virtual District District { set; get; }

		[Required]
		public int SortOrder { set; get; }

		public bool IsPublished { set; get; }

		public bool IsDeleted { set; get; }
	}
}
