using System;
using System.Collections.Generic;
using BowlingStats.Models;
using System.Linq;
using BowlingStats.Utils;
using Xamarin.Forms;
using BowlingStats.Views;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BowlingStats.ViewModels
{
    public class TournamentDetailViewModel : BaseViewModel
    {
        public TournamentModel Tournament { get; set; }
        public Command LoadTournamentGamesCommand { get; set; }

        public TournamentDetailViewModel(TournamentModel tournament = null)
        {
            Title = tournament?.TournamentResume;
            Tournament = tournament;
            LoadTournamentGamesCommand = new Command(async () => await ExecuteLoadTournamentGamesCommand());

            MessagingCenter.Unsubscribe<GameDetailPage, GameModel>(typeof(TournamentDetailViewModel), "SaveGame");
            MessagingCenter.Unsubscribe<TournamentDetailPage, GameModel>(typeof(TournamentDetailViewModel), "DeleteGame");

            MessagingCenter.Subscribe<GameDetailPage, GameModel>(typeof(TournamentDetailViewModel), "SaveGame", async (obj, item) =>
            {
                var newItem = item as GameModel;
                var oldItem = tournament.Games != null ? tournament.Games.Where(x => x.ID == item.ID).FirstOrDefault() : null;

                if (DataStore.SaveTournamentGame(newItem, tournament.ID))
                {
                    if (item.ID == 0)
                    {
                        if (tournament.Games == null)
                            tournament.Games = new System.Collections.ObjectModel.ObservableCollection<GameModel>();
                        tournament.Games.Add(newItem);
                    }
                    else
                    {
                        if (oldItem != null)
                            tournament.Games.Remove(oldItem);
                        tournament.Games.Add(newItem);
                    }
                }
            });

            MessagingCenter.Subscribe<TournamentDetailPage, GameModel>(typeof(TournamentDetailViewModel), "DeleteGame", async (obj, item) =>
            {
                var itemToDelete = item as GameModel;

                if (DataStore.DeleteTournamentGame(itemToDelete.ID))
                {
                    tournament.Games.Remove(itemToDelete);
                }
            });
        }

        public List<string> TournamentTypes
        {
            get
            {
                return (from Enums.TournamentTypeEnum n in Enum.GetValues(typeof(Enums.TournamentTypeEnum))
                        select Enumerations.GetEnumDescription(n)).ToList();
            }
        }

        public List<BowlingCenterModel> BowlingCenters
        {
            get
            {
                return App.Database.GetBowlingCentersAsync().Result.Select(x => new BowlingCenterModel
                {
                    ID = x.ID,
                    Name = x.Name,
                    City = x.City,
                    Address = x.Address
                }).ToList();
            }
        }

        async Task ExecuteLoadTournamentGamesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Tournament.Games == null)
                    Tournament.Games = new System.Collections.ObjectModel.ObservableCollection<GameModel>();

                Tournament.Games.Clear();
                var tournamentGames = await DataStore.GetTournamentGamesAsync(Tournament.ID);

                foreach (var tournamentGame in tournamentGames)
                {
                    Tournament.Games.Add(tournamentGame);
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
    }
}
