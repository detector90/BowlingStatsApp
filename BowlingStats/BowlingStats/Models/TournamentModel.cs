using BowlingStats.Enums;
using BowlingStats.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BowlingStats.Models
{
    public class TournamentModel
    {
        public int ID { get; set; }
        public TournamentTypeEnum TournamentType { get; set; }
        public BowlingCenterModel BowlingCenter { get; set; }
        public int Handicap { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsOfficial { get; set; }
        

        public ObservableCollection<GameModel> Games
        {
            get;
            set;
        }

        public void AddGame(GameModel game)
        {
            if (Games == null)
                Games = new ObservableCollection<GameModel>();
            
            Games.Add(game);
        }

        private async Task DeleteGame(GameModel game)
        {
            MessagingCenter.Send(this, "DeleteGame", game);
        }

        public string TournamentResume
        {
            get
            {
                return string.Concat(EventDate.ToString("dd/MM/yyyy"), " - ", BowlingCenter.City, " - ", Enumerations.GetEnumDescription(TournamentType));
            }
        }

        public Color RowColor
        {
            get
            {
                return IsOfficial ? Color.Yellow : Color.White;
            }
        }

        public Color TitleColor
        {
            get
            {
                return IsOfficial ? Color.Red : Color.Black;
            }
        }
    }
}
