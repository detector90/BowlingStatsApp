using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using BowlingStats.Models;
using BowlingStats.Views;

namespace BowlingStats.ViewModels
{
    public class BowlingCentersViewModel : BaseViewModel
    {
        public ObservableCollection<BowlingCenterModel> BowlingCenters { get; set; }
        public Command LoadBowlingCentersCommand { get; set; }

        public BowlingCentersViewModel()
        {
            Title = "Elenco centri bowling";
            BowlingCenters = new ObservableCollection<BowlingCenterModel>();
            LoadBowlingCentersCommand = new Command(async () => await ExecuteLoadBowlingCentersCommand());

            MessagingCenter.Unsubscribe<BowlingCenterDetailPage, BowlingCenterModel>(typeof(BowlingCentersViewModel), "SaveBowlingCenter");

            MessagingCenter.Subscribe<BowlingCenterDetailPage, BowlingCenterModel>(typeof(BowlingCentersViewModel), "SaveBowlingCenter", async (obj, item) =>
            {
                var newItem = item as BowlingCenterModel;
                bool result;

                if (item.ID == 0)
                {
                    BowlingCenters.Add(newItem);
                    await DataStore.AddBowlingCenterAsync(newItem);
                } else
                {
                    await DataStore.UpdateBowlingCenterAsync(newItem);

                    BowlingCenterModel bowlingCenterToUpdate = null;

                    foreach (BowlingCenterModel bowlingCenter in BowlingCenters)
                    {
                        if (bowlingCenter.ID == item.ID)
                            bowlingCenterToUpdate = bowlingCenter;
                    }

                    BowlingCenters.Remove(bowlingCenterToUpdate);
                    BowlingCenters.Add(item);
                }
                
            });
        }

        async Task ExecuteLoadBowlingCentersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                BowlingCenters.Clear();
                var bowlingCenters = await DataStore.GetBowlingCentersAsync(true);
                foreach (var bowlingCenter in bowlingCenters)
                {
                    bowlingCenter.DeleteBowlingCenterCommand = new Command(async () => await DeleteBowlingCenter(bowlingCenter));
                    BowlingCenters.Add(bowlingCenter);
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

        private async Task DeleteBowlingCenter(BowlingCenterModel bcToDelete)
        {
            bool confirm = await App.Current.MainPage.DisplayAlert("Conferma", "Sei sicuro di voler cancellare il centro bowling selezionato?", "Si", "No");

            if (confirm)
            {
                bool deleted = await DataStore.DeleteBowlingCenterAsync(bcToDelete.ID);

                if (deleted)
                {
                    await App.Current.MainPage.DisplayAlert("Cancellazione", "Cancellazione avvenuta con successo!", "ok");
                    BowlingCenters.Remove(bcToDelete);
                }
            }
        }
    }
}