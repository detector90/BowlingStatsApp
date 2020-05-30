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
    public partial class StatisticsPage : MyContentPage
    {
        StatisticsViewModel viewModel;

        public StatisticsPage()
        {
            InitializeComponent();
            viewModel = new StatisticsViewModel();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel = new StatisticsViewModel();
            BindingContext = viewModel;
        }
    }
}