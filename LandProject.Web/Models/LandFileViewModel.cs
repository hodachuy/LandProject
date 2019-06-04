using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class LandFileViewModel
    {
        public int ID { set; get; }

        [StringLength(250)]
        public string FileName { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Extension { set; get; }

        public string Url { set; get; }

        public int? LProjectID { set; get; }
        public int? PostID { set; get; }
        public int? LandNewsID { set; get; }
    }
}