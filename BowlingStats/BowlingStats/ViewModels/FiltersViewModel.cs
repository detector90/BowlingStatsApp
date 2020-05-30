using BowlingStats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingStats.ViewModels
{
    public class FiltersViewModel : BaseViewModel
    {
        public Filters Filters { get; set; }

        public FiltersViewModel()
        {
            Title = "Filtri di ricerca";

            Filters = new Filters()
            {
                BowlingCenter = App.Filter.BowlingCenter,
                DateFrom = App.Filter.DateFrom,
                DateTo = App.Filter.DateTo
            };
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
    }
}
