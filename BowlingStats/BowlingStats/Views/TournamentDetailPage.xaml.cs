using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BowlingStats.Models;
using BowlingStats.ViewModels;
using System.Linq;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TournamentDetailPage : ContentPage
    {
        TournamentDetailViewModel viewModel;

        public TournamentDetailPage(TournamentDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            var selectedBowling = viewModel.Tournament.BowlingCenter != null ? viewModel.BowlingCenters.Where(x => x.ID == viewModel.Tournament.BowlingCenter.ID).FirstOrDefault() : null;
            //BowlingCentersList.Items[0].Select(); //.SelectedIndex = selectedBowling != null ? viewModel.BowlingCenters.IndexOf(selectedBowling) : 0;
            BowlingCentersList.SelectedIndex = viewModel.BowlingCenters.FindIndex(x => x.ID == selectedBowling.ID);
            //BowlingCentersList.SelectedItem = selectedBowling;
        }

        public TournamentDetailPage()
        {
            InitializeComponent();

            var tournament = new TournamentModel
            {
                TournamentType = Enums.TournamentTypeEnum.Training,
                Handicap = 0,
                Description = "Descrizione torneo",
                EventDate = DateTime.UtcNow,
                IsOfficial = false
            };

            viewModel = new TournamentDetailViewModel(tournament);
            BindingContext = viewModel;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "SaveTournament", viewModel.Tournament);
            await Navigation.PopAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void AddGame_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new GameDetailPage(viewModel.Tournament.ID)));
        }

        async void EditGame_Clicked(object sender, EventArgs e)
        {
            GameModel selectedGame = GamesListView.SelectedItem as GameModel;
            await Navigation.PushModalAsync(new NavigationPage(new GameDetailPage(selectedGame, viewModel.Tournament.ID)));
        }

        void DeleteGame_Clicked(object sender, EventArgs e)
        {
            GameModel selectedGame = GamesListView.SelectedItem as GameModel;
            MessagingCenter.Send(this, "DeleteGame", selectedGame);
        }

        private void GamesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DeleteGame.IsVisible = true;
            EditGame.IsVisible = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DeleteGame.IsVisible = false;
            EditGame.IsVisible = false;

            //if (viewModel.Tournament.Games == null || viewModel.Tournament.Games.Count == 0)
            viewModel.LoadTournamentGamesCommand.Execute(null);
        }


    }
}