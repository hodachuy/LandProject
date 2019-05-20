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
    [Table("LandNewss")]
    public class LandNews : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string Alias { set; get; }

        public string Description { set; get; }

        [StringLength(256)]
        public string Code { set; get; }

        [StringLength(256)]
        [Required]
        public string Address { set; get; }

        [Required]
        public int LandTypeID { set; get; }

        [Required]
        public int LandCategoryID { set; get; }

        [Required]
        public int ProvinceID { set; get; }

        [Required]
        public int DistrictID { set; get; }

        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string UserID { set; get; }

        public string Image { set; get; }

        public int LandNewsScheduleID { set; get; }

        public int AgentID { set; get; }

        public int? WardID { set; get; }

        public int? LProjectID { set; get; }
        public int? Area { set; get; }
        public decimal? Price { set; get; }
        public string TotalPrice { set; get; }
        public decimal? DecimalTotalPrice { set; get; }
        public string Unit { set; get; }

        //Thông tin khác
        // Mặt tiền m
        public int? Facade { set; get; }

        // Đường vào m
        public int? Entry { set; get; }

        [StringLength(50)]
        public string HouseDirection { set; get; }
        [StringLength(50)]
        public string BalconyDirection {set;get;}
        public int? NumberFloor { set; get; }
        public int? NumberBedroom { set; get; }
        public int? NumberWC { set; get; }
        // nooi that
        public string Furniture { set; get; }

        [MaxLength(50)]
		public string LatiLongTude { set; get; }

		public bool IsDelete { set; get; }

		public bool IsPublished { set; get; }

		public bool IsSale { set; get; }

	}
}
