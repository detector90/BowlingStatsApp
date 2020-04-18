using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingStats.Models
{
    public class BowlingCenterModel
    {
        public int ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Resume
        {
            get {
                return string.Concat(City, " - ", Name);
            }
        }
    }
}
