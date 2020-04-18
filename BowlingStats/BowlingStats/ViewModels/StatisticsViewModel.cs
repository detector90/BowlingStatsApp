﻿using BowlingStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingStats.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        public Statistics GlobalStatistics { get; set; }
        public Statistics OfficialStatistics { get; set; }
        public Statistics UnofficialStatistics { get; set; }

        public StatisticsViewModel()
        {
            IEnumerable<GameModel> gameModels = DataStore.GetAllGames(Enums.OfficialFilterEnum.All).Result;
            GlobalStatistics = new Statistics();
            OfficialStatistics = new Statistics();
            UnofficialStatistics = new Statistics();

            int maxGame;
            GameModel highestGame;
            int maxHdpGame;
            GameModel highestHdpGame;

            if (gameModels.Count() > 0)
            {
                GlobalStatistics.ScratchAverage = Math.Round(gameModels.Average(x => x.FinalScore), 2);
                GlobalStatistics.HdpAverage = Math.Round(gameModels.Average(x => x.FinalScoreHDP), 2);
                GlobalStatistics.PlayedGamesNumber = gameModels.Count();
                GlobalStatistics.Over2HundredGames = gameModels.Where(x => x.FinalScore > 200).Count();
                GlobalStatistics.Over2HundredGamesHdp = gameModels.Where(x => x.FinalScoreHDP > 200).Count();

                maxGame = gameModels.Max(x => x.FinalScore);
                highestGame = gameModels.Where(x => x.FinalScore == maxGame).First();

                GlobalStatistics.ScratchHighestGame = string.Concat(highestGame.FinalScore, " (", highestGame.TournamentResume, ")");

                maxHdpGame = gameModels.Max(x => x.FinalScoreHDP);
                highestHdpGame = gameModels.Where(x => x.FinalScoreHDP == maxHdpGame).First();

                GlobalStatistics.HdpHighestGame = string.Concat(highestHdpGame.FinalScoreHDP, " (", highestHdpGame.TournamentResume, ")");
            }

            gameModels = DataStore.GetAllGames(Enums.OfficialFilterEnum.OnlyOfficial).Result;

            if (gameModels.Count() > 0)
            {
                OfficialStatistics.ScratchAverage = Math.Round(gameModels.Average(x => x.FinalScore), 2);
                OfficialStatistics.HdpAverage = Math.Round(gameModels.Average(x => x.FinalScoreHDP), 2);
                OfficialStatistics.PlayedGamesNumber = gameModels.Count();
                OfficialStatistics.Over2HundredGames = gameModels.Where(x => x.FinalScore > 200).Count();
                OfficialStatistics.Over2HundredGamesHdp = gameModels.Where(x => x.FinalScoreHDP > 200).Count();

                maxGame = gameModels.Max(x => x.FinalScore);
                highestGame = gameModels.Where(x => x.FinalScore == maxGame).First();

                OfficialStatistics.ScratchHighestGame = string.Concat(highestGame.FinalScore, " (", highestGame.TournamentResume, ")");

                maxHdpGame = gameModels.Max(x => x.FinalScoreHDP);
                highestHdpGame = gameModels.Where(x => x.FinalScoreHDP == maxHdpGame).First();

                OfficialStatistics.HdpHighestGame = string.Concat(highestHdpGame.FinalScoreHDP, " (", highestHdpGame.TournamentResume, ")");
            }

            gameModels = DataStore.GetAllGames(Enums.OfficialFilterEnum.OnlyUnofficial).Result;

            if (gameModels.Count() > 0)
            {
                UnofficialStatistics.ScratchAverage = Math.Round(gameModels.Average(x => x.FinalScore), 2);
                UnofficialStatistics.HdpAverage = Math.Round(gameModels.Average(x => x.FinalScoreHDP), 2);
                UnofficialStatistics.PlayedGamesNumber = gameModels.Count();
                UnofficialStatistics.Over2HundredGames = gameModels.Where(x => x.FinalScore > 200).Count();
                UnofficialStatistics.Over2HundredGamesHdp = gameModels.Where(x => x.FinalScoreHDP > 200).Count();

                maxGame = gameModels.Max(x => x.FinalScore);
                highestGame = gameModels.Where(x => x.FinalScore == maxGame).First();

                UnofficialStatistics.ScratchHighestGame = string.Concat(highestGame.FinalScore, " (", highestGame.TournamentResume, ")");

                maxHdpGame = gameModels.Max(x => x.FinalScoreHDP);
                highestHdpGame = gameModels.Where(x => x.FinalScoreHDP == maxHdpGame).First();

                UnofficialStatistics.HdpHighestGame = string.Concat(highestHdpGame.FinalScoreHDP, " (", highestHdpGame.TournamentResume, ")");
            }



        }
    }
}
