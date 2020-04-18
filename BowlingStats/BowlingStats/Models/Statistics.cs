using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingStats.Models
{
    public class Statistics
    {
        public double ScratchAverage { get; set; }
        public double HdpAverage { get; set; }
        public int PlayedGamesNumber { get; set; }
        public int Over2HundredGames { get; set; }
        public int Over2HundredGamesHdp { get; set; }
        public string ScratchHighestGame { get; set; }
        public string HdpHighestGame { get; set; }
    }
}
