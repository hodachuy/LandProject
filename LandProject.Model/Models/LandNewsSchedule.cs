using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
    [Table("LandNewsSchedules")]
	public class LandNewsSchedule
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [MaxLength(200)]
        [Required]
        public string Name { set; get; }

        [MaxLength(200)]
        public string Alias { set; get; }

        public string Description { set; get; }

        public DateTime? StartDate { set; get; }

        public DateTime? EndDate { set; get; }

        public decimal Price { set; get; }
    }
}
