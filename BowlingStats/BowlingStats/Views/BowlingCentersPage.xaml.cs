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
    public partial class BowlingCentersPage : MyContentPage
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
            await Navigation.PushModalAsync(new NavigationPage(new BowlingCenterDetailPage(bowlingCenter)));
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