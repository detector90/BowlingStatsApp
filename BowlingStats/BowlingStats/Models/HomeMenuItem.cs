using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingStats.Models
{
    public enum MenuItemType
    {
        EventsList,
        Statistics,
        BowlingCenters,
        Filters
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }
    }
}
