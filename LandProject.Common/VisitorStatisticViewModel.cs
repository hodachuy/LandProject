using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandProject.Common
{
    public class VisitorStatisticViewModel
    {
        public DateTime Date { set; get; }
        public int CountVisitors { set; get; }
    }

    public class VisitorStatisticTotalViewModel
    {
        public int TotalInDay { set; get; }
        public int TotalAllDay { set; get; }
    }
}
