using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BowlingStats.Entities
{
    public class Game
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [ForeignKey("Tournament")]
        public int TournamentID { get; set; }
        public int GameOrderID { get; set; }
        public int Score { get; set; }
    }
}
