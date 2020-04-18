using BowlingStats.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingStats.Utils
{
    public static class BusinessLogic
    {
        public static int ProposedGameOrderID(int tournamentID)
        {
            List<Game> tournamentGames = App.Database.GetTournamentGames(tournamentID).Result;

            if (tournamentGames.Count == 0)
                return 1;

            int lastGameOrderID = tournamentGames.Max(x => x.GameOrderID);
            int proposedGameOrderID = 1;
            bool found = false;

            for (int i = 1; i <= lastGameOrderID && !found; i++)
            {
                proposedGameOrderID = i;
                Game nGame = tournamentGames.Where(x => x.GameOrderID == i).FirstOrDefault();

                if (nGame == null)
                {
                    found = true;
                    break;
                }
            }

            return found ? proposedGameOrderID : ++proposedGameOrderID;
        }
    }
}
