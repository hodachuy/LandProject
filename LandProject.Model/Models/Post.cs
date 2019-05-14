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
	public class Post: Auditable
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { set; get; }

		[Required]
		[MaxLength(256)]
		public string Name { set; get; }

		[Required]
		[MaxLength(256)]
		[Column(TypeName = "varchar")]
		public string Alias { set; get; }

		[Required]
		public int PostCategoryID { set; get; }

		[MaxLength(256)]
		public string Image { set; get; }

		[MaxLength(500)]
		public string Description { set; get; }

		public string Content { set; get; }

		public bool IsPublished { set; get; }
		public bool IsDelete { set; get; }
		public int? ViewCount { set; get; }

		[ForeignKey("PostCategoryID")]
		public virtual PostCategory PostCategory { set; get; }

		public virtual IEnumerable<PostTag> PostTags { set; get; }
	}
}
