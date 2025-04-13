using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DailyData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public List<string> ToDo { get; set; }

        public override string ToString()
        {
            return $"{Year}.{Month:D2}.{Day:D2}";
        }
    }
}
