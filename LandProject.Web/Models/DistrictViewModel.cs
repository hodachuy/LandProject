using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class DistrictViewModel
    {
        public int ID { set; get; }

        [MaxLength(250)]
        public string Name { set; get; }

        [MaxLength(50)]
        public string Type { set; get; }

        [MaxLength(50)]
        public string LatiLongTude { set; get; }

        [Required]
        public int ProvinceID { set; get; }

        public virtual ProvinceViewModel Province { set; get; }

        public int SortOrder { set; get; }

        public bool IsPublished { set; get; }

        public bool IsDeleted { set; get; }

        public virtual IEnumerable<WardViewModel> Wards { set; get; }
    }
}