using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
    [Table("LandTypes")]
	public class LandType
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

		[MaxLength(250)]
        public string Description { set; get; }

        public int SortOrder { set; get; }

        public int TypeExchange { set; get; }

        public bool IsDelete { set; get; }

        public virtual IEnumerable<LandCategory> LandCategories { set; get; }
    }
}
