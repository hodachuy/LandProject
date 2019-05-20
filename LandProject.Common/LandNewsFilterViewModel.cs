using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LandProject.Common
{
    public class LandNewsFilterViewModel
    {
        public int ID { set; get; }

        public string Title { set; get; }

        public string Alias { set; get; }

        public string Description { set; get; }

        public string Image { set; get; }

        public string Code { set; get; }

        public string Address { set; get; }

        public int LandTypeID { set; get; }
        public string LandTypeName { set; get; }

        public int LandCategoryID { set; get; }
        public string LandCategoryName { set; get; }

        public int ProvinceID { set; get; }
        public string ProvinceName { set; get; }

        public int DistrictID { set; get; }

        public string DistrictName { set; get; }

        public string UserID { set; get; }

        public string UserName { set; get; }

        public int LandNewsScheduleID { set; get; }

        public int AgentID { set; get; }
        public string AgentName { set; get; }

        public int? WardID { set; get; }

        public string WardName { set; get; }

        public int? LProjectID { set; get; }

        public string LProjectName { set; get; }

        public int? Area { set; get; }

        public decimal? Price { set; get; }

        public string TotalPrice { set; get; }

        public decimal? DecimalTotalPrice2 { set; get; }

        public string Unit { set; get; }

        public int? Facade { set; get; }

        public int? Entry { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string HouseDirection { set; get; }

        public string BalconyDirection { set; get; }
        public int? NumberFloor { set; get; }
        public int? NumberBedroom { set; get; }
        public int? NumberWC { set; get; }
        public string Furniture { set; get; }

        public string LatiLongTude { set; get; }

        public bool IsDelete { set; get; }

        public bool IsPublished { set; get; }

        public bool IsSale { set; get; }

        public int Total { set; get; }
    }
}