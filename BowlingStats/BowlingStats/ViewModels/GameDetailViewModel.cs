using BowlingStats.Models;
using BowlingStats.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BowlingStats.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        public GameModel Game { get; set; }
        public string Title { get; set; }
        public int TournamentID { get; set; }
        public Command ButtonClickedCommand { get; set; }

        public GameDetailViewModel(int tournamentID)
        {
            Title = "Nuova partita";

            Game = new GameModel();
            Game.GameOrderID = BusinessLogic.ProposedGameOrderID(tournamentID);
            TournamentID = tournamentID;
        }

        public GameDetailViewModel(GameModel game, int tournamentID)
        {
            Title = "Dettaglio partita";

            Game = game;
            TournamentID = tournamentID;
        }
    }
}
