﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Model.Models
{
    [Table("Agents")]
    public class Agent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [StringLength(250)]
        public string Name { set; get; }

        [StringLength(256)]
        public string Address { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Phone { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Required]
        public string Mobile { set; get; }

        [Column(TypeName = "varchar")]
        [StringLength(128)]
        public string Email { set; get; }
    }
}
