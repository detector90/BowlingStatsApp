using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BowlingStats.Models;
using System.Linq;
using BowlingStats.Utils;
using BowlingStats.Entities;

namespace BowlingStats.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewTournamentPage : MyContentPage
    {
        public TournamentModel Tournament { get; set; }
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
                return App.Database.GetBowlingCentersAsync().Result.Select(x => new BowlingCenterModel {
                    ID = x.ID,
                    Name = x.Name,
                    City = x.City,
                    Address = x.Address
                }).ToList();
            }
        }

        public NewTournamentPage()
        {
            InitializeComponent();

            Tournament = new TournamentModel
            {
                TournamentType = Enums.TournamentTypeEnum.Training,
                Handicap = 0,
                Description = "Descrizione torneo",
                EventDate = DateTime.UtcNow,
                IsOfficial = false
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string errorMessage = DataValidator.Validate(Tournament).ErrorMessage;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                await DisplayAlert("Errore", errorMessage, "OK");
                return;
            }

            MessagingCenter.Send(this, "AddTournament", Tournament);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}