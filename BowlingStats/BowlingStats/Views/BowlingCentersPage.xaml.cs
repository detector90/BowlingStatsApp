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
    public partial class BowlingCentersPage : ContentPage
    {
        BowlingCentersViewModel viewModel;

        public BowlingCentersPage()
        {
            InitializeComponent();
            viewModel = new BowlingCentersViewModel();
            BindingContext = viewModel;
        }

        async void OnBowlingCenterSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var bowlingCenter = args.SelectedItem as BowlingCenterModel;
            if (bowlingCenter == null)
                return;

            string result = await DisplayActionSheet("Cosa desideri fare?", "Annulla", null, new string[] { "- Modifica centro bowling", "- Cancella centro bowling" });

            switch (result)
            {
                case "Annulla":
                    break;

                case "- Cancella centro bowling":
                    bool deleted = await viewModel.DataStore.DeleteBowlingCenterAsync(bowlingCenter.ID);

                    if (deleted)
                    {
                        await DisplayAlert("Cancellazione", "Cancellazione avvenuta con successo!", "ok");
                        viewModel.LoadBowlingCentersCommand.Execute(null);
                    }
                    break;

                case "- Modifica centro bowling":
                    await Navigation.PushModalAsync(new NavigationPage(new BowlingCenterDetailPage(bowlingCenter)));
                    break;

                default:
                    break;
            }
        }

        async void AddBowlingCenter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new BowlingCenterDetailPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadBowlingCentersCommand.Execute(null);
        }
    }
}