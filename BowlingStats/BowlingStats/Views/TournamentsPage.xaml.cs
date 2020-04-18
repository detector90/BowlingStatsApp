using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BowlingStats.Models;
using BowlingStats.Views;
using BowlingStats.ViewModels;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TournamentsPage : ContentPage
    {
        TournamentsViewModel viewModel;

        public TournamentsPage()
        {
            InitializeComponent();
            viewModel = new TournamentsViewModel();
            BindingContext = viewModel;
        }

        async void OnTournamentSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var tournament = args.SelectedItem as TournamentModel;
            if (tournament == null)
                return;

            string result = await DisplayActionSheet("Cosa desideri fare?", "Annulla", null, new string[] { "- Modifica evento", "- Cancella evento" });

            switch (result)
            {
                case "Annulla":
                    break;

                case "- Cancella evento":
                    bool deleted = await viewModel.DataStore.DeleteTournamentAsync(tournament.ID);

                    if (deleted)
                    {
                        await DisplayAlert("Cancellazione", "Cancellazione avvenuta con successo!", "ok");
                        viewModel.LoadTournamentsCommand.Execute(null);
                    }
                    break;

                case "- Modifica evento":
                    await Navigation.PushAsync(new TournamentDetailPage(new TournamentDetailViewModel(tournament)));
                    break;

                default:
                    break;
            }
        }

        async void AddTournament_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewTournamentPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadTournamentsCommand.Execute(null);
        }
    }
}