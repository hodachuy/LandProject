using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
	[Table("Districts")]
	public class District
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { set; get; }

		[MaxLength(250)]
		public string Name { set; get; }

		[MaxLength(50)]
		public string Type { set; get; }

		[MaxLength(50)]
		public string LatiLongTude { set; get; }

		[Required]
		public int ProvinceID { set; get; }

        [ForeignKey("ProvinceID")]
        public virtual Province Province { set; get; }

		public int SortOrder { set; get; }

		public bool IsPublished { set; get; }

		public bool IsDeleted { set; get; }

        public virtual IEnumerable<Ward> Wards { set; get; }
	}
}
