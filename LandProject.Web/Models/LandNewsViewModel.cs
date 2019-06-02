using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class LandNewsViewModel
    {
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string Alias { set; get; }

        public string Description { set; get; }

        public string Image { set; get; }

        [StringLength(256)]
        public string Code { set; get; }

        [StringLength(256)]
        [Required]
        public string Address { set; get; }

        [Required]
        public int LandTypeID { set; get; }
        public string LandTypeName { set; get; }

        [Required]
        public int LandCategoryID { set; get; }
        public string LandCategoryName { set; get; }

        [Required]
        public int ProvinceID { set; get; }
        public string ProvinceName { set; get; }

        [Required]
        public int DistrictID { set; get; }
        public string DistrictName { set; get; }

        [StringLength(128)]
        [Column(TypeName = "nvarchar")]
        public string UserID { set; get; }

        public int LandNewsScheduleID { set; get; }
        public string LandNewsScheduleName { set; get; }

        public int AgentID { set; get; }

        public int? WardID { set; get; }
        public string WardName { set; get; }

        public int? LProjectID { set; get; }
        public string LProjectName { set; get; }

        public int? Area { set; get; }

        public decimal? Price { set; get; }

        public string TotalPrice { set; get; }

        public decimal? DecimalTotalPrice { set; get; }

        public string Unit { set; get; }

        public int? Facade { set; get; }

        public int? Entry { set; get; }

        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }

        [MaxLength(256)]
        public string MetaKeyword { set; get; }

        [MaxLength(256)]
        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        [StringLength(50)]
        public string HouseDirection { set; get; }
        [StringLength(50)]
        public string BalconyDirection { set; get; }
        public int? NumberFloor { set; get; }
        public int? NumberBedroom { set; get; }
        public int? NumberWC { set; get; }
        public string Furniture { set; get; }

		// phap ly
		public string LegalStatus { set; get; }
		// tiện nghi
		public string Convenient { set; get; }
		// môi trường
		public string Environment { set; get; }

		[MaxLength(50)]
        public string LatiLongTude { set; get; }

        public bool IsDelete { set; get; }

        public bool IsPublished { set; get; }

        public bool IsSale { set; get; }

		public int LandTypeSortOrder { set; get; }

		public DateTime? PublishedDate { set; get; }

		public virtual IEnumerable<LandFileViewModel> LandFiles { set; get; }
        public virtual AgentViewModel Agent  { set; get; }
    }
}