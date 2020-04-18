using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BowlingStats.Entities
{
    public class Tournament
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey("BowlingCenter")]
        public int BowlingCenterID { get; set; }
        public int Type { get; set; }
        public int Handicap { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsOfficial { get; set; }
    }
}
