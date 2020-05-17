using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BowlingStats.Entities
{
    public class Frame
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey("Game")]
        public int GameID { get; set; }
        public int FrameOrderID { get; set; }
        public int FirstAttempt { get; set; }
        public int SecondAttempt { get; set; }
        public int ThirdAttempt { get; set; }
        public bool IsStrike { get; set; }
        public bool IsSpare { get; set; }
        public bool IsSplit { get; set; }
    }
}
