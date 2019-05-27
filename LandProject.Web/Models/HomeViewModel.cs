﻿using LandProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CountLandNewsDistrictViewModel> CategoryWard { set; get; }
        public IEnumerable<LandNewsFilterViewModel> LandNewsSaleAndRent { set; get; }
        public IEnumerable<LandNewsFilterViewModel> LandNewsBuyAndRent { set; get; }
        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }
    }
}