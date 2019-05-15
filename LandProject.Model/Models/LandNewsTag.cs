using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
    [Table("LandNewsTags")]
    public class LandNewsTag
    {
        [Key]
        [Column(Order = 1)]
        public int LandNewsID { set; get; }

        [Key]
        [Column(TypeName = "varchar", Order = 2)]
        [MaxLength(50)]
        public string TagID { set; get; }

        [ForeignKey("LandNewsID")]
        public virtual LandNews LandNews { set; get; }

        [ForeignKey("TagID")]
        public virtual Tag Tag { set; get; }
    }
}
