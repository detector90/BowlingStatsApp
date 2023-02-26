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
        [Description("Torneo federale")]
        FederalEvent = 1,
        [Description("Torneo volante")]
        SociableEvent = 2,
        [Description("Torneo monetario")]
        MonetaryEvent = 3
    }
}
