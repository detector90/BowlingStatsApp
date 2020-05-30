using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingStats.Models
{
    public class Filters
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public BowlingCenterModel BowlingCenter { get; set; }
    }
}
