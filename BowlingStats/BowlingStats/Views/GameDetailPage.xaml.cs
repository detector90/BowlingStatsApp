using System;
using System.ComponentModel;
using Xamarin.Forms;

using BowlingStats.Models;
using BowlingStats.Utils;
using System.Collections.ObjectModel;
using BowlingStats.ViewModels;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class GameDetailPage : MyContentPage
    {
        public GameDetailViewModel viewModel;

        public GameDetailPage(int tournamentID)
        {
            InitializeComponent();

            viewModel = new GameDetailViewModel(tournamentID);

            viewModel.ButtonClickedCommand = new Command<string>(
            execute: (string arg) =>
            {
                int pins = viewModel.SetFramePointAndReturnRemainingPins(arg);
                SetPinButtonsVisibility(pins);
            },
            canExecute: (string arg) =>
            {
                return true;
            });

            viewModel.FrameSelectedCommand = new Command<string>(
            execute: (string arg) =>
            {
                SetPinButtonsVisibility(10);
                viewModel.SetSelected(false);
                viewModel.CurrentFrame = Int32.Parse(arg);
                viewModel.CurrentAttempt = 1;
                viewModel.SetSelected(true, viewModel.CurrentFrame);
            },
            canExecute: (string arg) =>
            {
                return true;
            });

            BindingContext = viewModel;
        }

        public GameDetailPage(GameModel game, int tournamentID)
        {
            InitializeComponent();

            GameModel Game = game;
            viewModel = new GameDetailViewModel(game, tournamentID);

            viewModel.ButtonClickedCommand = new Command<string>(
            execute: (string arg) =>
            {
                int pins = viewModel.SetFramePointAndReturnRemainingPins(arg);
                SetPinButtonsVisibility(pins);
            },
            canExecute: (string arg) =>
            {
                return true;
            });

            viewModel.FrameSelectedCommand = new Command<string>(
            execute: (string arg) =>
            {
                SetPinButtonsVisibility(10);
                viewModel.SetSelected(false);
                viewModel.CurrentFrame = Int32.Parse(arg);
                viewModel.CurrentAttempt = 1;
                viewModel.SetSelected(true, viewModel.CurrentFrame);
            },
            canExecute: (string arg) =>
            {
                return true;
            });

            BindingContext = viewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string errorMessage = DataValidator.Validate(viewModel.Game, viewModel.TournamentID).ErrorMessage;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                await DisplayAlert("Errore", errorMessage, "OK");
                return;
            }

            MessagingCenter.Send(this, "SaveGame", viewModel.Game);
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
    }
}