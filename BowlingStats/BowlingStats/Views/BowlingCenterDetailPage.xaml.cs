using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BowlingStats.Models;
using System.Linq;
using BowlingStats.Utils;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class BowlingCenterDetailPage : ContentPage
    {
        public BowlingCenterModel BowlingCenter { get; set; }
        public string Title { get; set; }

        public BowlingCenterDetailPage()
        { 
            InitializeComponent();

            BowlingCenter = new BowlingCenterModel();

            Title = "Nuovo centro bowling";
            
            BindingContext = this;
        }

        public BowlingCenterDetailPage(BowlingCenterModel bowlingCenter)
        {
            InitializeComponent();

            BowlingCenter = bowlingCenter;
            Title = "Dettaglio centro bowling";

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "SaveBowlingCenter", BowlingCenter);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}