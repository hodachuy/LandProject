using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class ProvinceViewModel
    {
        public int ID { set; get; }

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(20)]
        public string Type { set; get; }

        public int CountryID { set; get; }

        public string CountryCode { set; get; }

        public int TelephoneCode { set; get; }

        [MaxLength(20)]
        public string ZipCode { set; get; }

        public int SortOrder { set; get; }

        public bool IsPublished { set; get; }

        public bool IsDeleted { set; get; }

        public virtual IEnumerable<DistrictViewModel> Districts { set; get; }
    }
}