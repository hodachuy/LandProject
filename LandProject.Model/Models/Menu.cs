using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandProject.Model.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

        [Required]
        [MaxLength(250)]
        public string URL { set; get; }

        public string Alias { set; get; }
        public int? DisplayOrder { set; get; }

		public string Image { set; get; }

		public int? ParentID { set; get; }

		[Required]
        public int MenuGroupID { set; get; }

        [ForeignKey("MenuGroupID")]
        public virtual MenuGroup MenuGroup { set; get; }

        [MaxLength(10)]
        public string Target { set; get; }

        public bool Status { set; get; }

    }
}
