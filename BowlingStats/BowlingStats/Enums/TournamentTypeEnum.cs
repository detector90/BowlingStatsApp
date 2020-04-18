using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BowlingStats.Enums
{
    public enum TournamentTypeEnum
    {
        [Description("Allenamento")]
        Training = 0,
        [Description("Torneo ufficiale")]
        OfficialEvent = 1,
        [Description("Torneo ufficiale interno")]
        InternalOfficialEvent = 2
    }
}
