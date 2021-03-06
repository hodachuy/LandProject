﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class AgentViewModel
    {
        public int ID { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(256)]
        public string Address { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Phone1 { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Phone2 { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Phone3 { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string PhoneShow { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(128)]
        public string Email { set; get; }

        public string UserId { set; get; }
    }
}