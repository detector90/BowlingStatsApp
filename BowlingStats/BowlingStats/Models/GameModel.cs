using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BowlingStats.Models
{
    public class GameModel
    {
        public int ID { get; set; }
        public int GameOrderID { get; set; }
        public int FinalScore { get; set; }
        public int FinalScoreHDP { get; set; }

        public Color TextColor
        {
            get
            {
                if (FinalScoreHDP < 200)
                    return Color.Black;
                else
                    if (FinalScoreHDP == 300)
                        return Color.DarkCyan;
                    else
                        return Color.Red;
            }
        }

        public string ScoresResume
        {
            get
            {
                return FinalScore + " (" + FinalScoreHDP + ")";
            }
        }

        public string TournamentResume { get; set; }
    }
}
