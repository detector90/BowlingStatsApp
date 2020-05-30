using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BowlingStats.Services;
using BowlingStats.Views;
using BowlingStats.Utils;
using System.IO;
using BowlingStats.Models;

namespace BowlingStats
{
    public partial class App : Application
    {
        static Database database;
        Filters filter;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BowlingStats_v6.db3"));
                }
                return database;
            }
        }

        public Filters Filter { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<TournamentDataStore>();
            Filter = new Filters()
            {
                BowlingID = 0,
                DateFrom = new DateTime(DateTime.Now.Year, 1, 1),
                DateTo = new DateTime(DateTime.Now.Year, 12, 31)
            };
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
