using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingStats.Entities
{
    public class BowlingCenter
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
