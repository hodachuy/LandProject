using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
	[Table("Provinces")]
	public class Province
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { set; get; }

		[MaxLength(250)]
		[Required]
		public string Name { set; get; }

		[MaxLength(20)]
		public string Type { set; get; }

		public int TelephoneCode { set; get; }

		[MaxLength(20)]
		public string ZipCode { set; get; }

		public int SortOrder { set; get; }
		
		public bool IsPublished { set; get; }

		public bool IsDeleted { set; get; }
	}
}
