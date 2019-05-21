using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Common
{
    public class CountLandNewsDistrictViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Type { set; get; }

        public string LatiLongTude { set; get; }

        public int DistrictID { set; get; }

        public int SortOrder { set; get; }

        public bool IsPublished { set; get; }

        public bool IsDeleted { set; get; }

        public int TotalLandNews { set; get; }
    }
}
