using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class WardViewModel
    {
        public int ID { set; get; }

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(50)]
        public string Type { set; get; }

        [MaxLength(50)]
        public string LatiLongTude { set; get; }

        [Required]
        public int DistrictID { set; get; }
        public virtual DistrictViewModel District { set; get; }

        [Required]
        public int SortOrder { set; get; }

        public bool IsPublished { set; get; }

        public bool IsDeleted { set; get; }
    }
}