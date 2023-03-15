using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using BowlingStats.Models;
using BowlingStats.Views;

namespace BowlingStats.ViewModels
{
    public class TournamentsViewModel : BaseViewModel
    {
        public ObservableCollection<TournamentModel> Tournaments { get; set; }
        public Command LoadTournamentsCommand { get; set; }
        public Command LongPressedCommand { get; set; }
        public Command DeleteTournamentCommand { get; set; }

        public TournamentsViewModel()
        {
            Title = "Elenco eventi";
            Tournaments = new ObservableCollection<TournamentModel>();
            LoadTournamentsCommand = new Command(async () => await ExecuteLoadTournamentsCommand());
            
            MessagingCenter.Unsubscribe<NewTournamentPage, TournamentModel>(typeof(TournamentsViewModel), "AddTournament");
            MessagingCenter.Unsubscribe<TournamentDetailPage, TournamentModel>(typeof(TournamentsViewModel), "SaveTournament");

            MessagingCenter.Subscribe<NewTournamentPage, TournamentModel>(typeof(TournamentsViewModel), "AddTournament", async (obj, item) =>
            {
                var newItem = item as TournamentModel;
                Tournaments.Add(newItem);
                await DataStore.AddTournamentAsync(newItem);
            });

            MessagingCenter.Subscribe<TournamentDetailPage, TournamentModel>(typeof(TournamentsViewModel), "SaveTournament", async (obj, item) =>
            {
                TournamentModel tournamentToRemove = null;

                foreach(TournamentModel tournament in Tournaments)
                {
                    if (tournament.ID == item.ID)
                        tournamentToRemove = tournament;
                }

                Tournaments.Remove(tournamentToRemove);
                Tournaments.Add(item);

                await DataStore.UpdateTournamentAsync(item, item.Games);
            });
        }

        async Task ExecuteLoadTournamentsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Tournaments.Clear();
                var tournaments = await DataStore.GetTournamentsAsync(true);
                foreach (var tournament in tournaments)
                {
                    tournament.DeleteTournamentCommand = new Command(async () => await DeleteTournament(tournament));
                    Tournaments.Add(tournament);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteTournament(TournamentModel tournamentToDelete)
        {
            bool confirm = await App.Current.MainPage.DisplayAlert("Conferma", "Sei sicuro di voler cancellare il torneo selezionato?", "Si", "No");

            if (confirm)
            {
                bool deleted = await DataStore.DeleteTournamentAsync(tournamentToDelete.ID);

                if (deleted)
                {
                    await App.Current.MainPage.DisplayAlert("Cancellazione", "Cancellazione avvenuta con successo!", "ok");
                    Tournaments.Remove(tournamentToDelete);
                }
            }
        }
    }
}