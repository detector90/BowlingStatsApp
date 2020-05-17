using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BowlingStats.Models;
using System.Linq;
using BowlingStats.Utils;
using System.Collections.ObjectModel;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class GameDetailPage : ContentPage
    {
        public GameModel Game { get; set; }
        public string Title { get; set; }
        private int TournamentID { get; set; }
        public Command ButtonClickedCommand { get; set; }

        public GameDetailPage(int tournamentID)
        {
            InitializeComponent();

            Game = new GameModel();

            Game.GameOrderID = BusinessLogic.ProposedGameOrderID(tournamentID);

            Title = "Nuova partita";
            TournamentID = tournamentID;

            ButtonClickedCommand = new Command<string>(
            execute: (string arg) =>
            {
                SetPinButtonsVisibility(Int32.Parse(arg));
            },
            canExecute: (string arg) =>
            {
                return true;
            });

            BindingContext = this;
        }

        public GameDetailPage(GameModel game, int tournamentID)
        {
            InitializeComponent();

            Game = game;
            Title = "Dettaglio partita";
            TournamentID = tournamentID;

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string errorMessage = GameValidation.Validate(Game, TournamentID).ErrorMessage;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                await DisplayAlert("Errore", errorMessage, "OK");
                return;
            }

            MessagingCenter.Send(this, "SaveGame", Game);
            await Navigation.PopModalAsync();
        }

        private void SetPinButtonsVisibility(int remainingPins)
        {
            for (int i = 0; i < 11; i++)
            {
                Button button = (Button)PinButtons.FindByName("Button" + i.ToString());

                if (i <= remainingPins)
                    button.IsVisible = true;
                else
                    button.IsVisible = false;
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void HasDetail_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox cbHasDetail = (CheckBox)sender;

            if (cbHasDetail.IsChecked && Game.Frames.Count == 0)
            {
                Game.Frames = new ObservableCollection<FrameModel>();

                for (int i = 0; i < 10; i++)
                {
                    Game.Frames.Add(new FrameModel());
                }
            }
        }
    }
}