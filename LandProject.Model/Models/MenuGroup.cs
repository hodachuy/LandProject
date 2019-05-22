using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandProject.Model.Models
{
    [Table("MenuGroups")]
    public class MenuGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

		[MaxLength(250)]
		public string URL { set; get; }

		[MaxLength(10)]
		public string Target { set; get; }

        public string Alias { set; get; }

        public bool Status { set; get; }

		public int? DisplayOrder { set; get; }

		public virtual IEnumerable<Menu> Menus { set; get; }
    }
}
