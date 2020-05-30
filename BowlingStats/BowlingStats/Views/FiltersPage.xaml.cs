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
    public partial class FiltersPage : MyContentPage
    {
        FiltersViewModel viewModel;

        public FiltersPage()
        {
            InitializeComponent();
            viewModel = new FiltersViewModel();
            BindingContext = viewModel;

            var selectedBowling = viewModel.Filters.BowlingCenter != null ? viewModel.BowlingCenters.Where(x => x.ID == viewModel.Filters.BowlingCenter.ID).FirstOrDefault() : null;
            if (selectedBowling != null)
                BowlingCenterPicker.SelectedIndex = viewModel.BowlingCenters.FindIndex(x => x.ID == selectedBowling.ID);
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            App.Filter = viewModel.Filters;
            DisplayAlert("Filtri aggiornati", "Aggiornamento filtri avvenuto con successo!", "ok");
        }

        void Reset_Clicked(object sender, EventArgs e)
        {
            App.ResetFilters();

            viewModel = new FiltersViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel = new FiltersViewModel();
            BindingContext = viewModel;

            var selectedBowling = viewModel.Filters.BowlingCenter != null ? viewModel.BowlingCenters.Where(x => x.ID == viewModel.Filters.BowlingCenter.ID).FirstOrDefault() : null;
            if (selectedBowling != null)
                BowlingCenterPicker.SelectedIndex = viewModel.BowlingCenters.FindIndex(x => x.ID == selectedBowling.ID);
        }
    }
}