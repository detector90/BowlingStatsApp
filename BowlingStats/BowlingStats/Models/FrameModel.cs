using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BowlingStats.Models
{
    public class FrameModel
    {
        public int ID { get; set; }
        public string FirstAttempt { get; set; }
        public string SecondAttempt { get; set; }
        public string ThirdAttempt { get; set; }
        public int DbFirstAttempt { get; set; }
        public int DbSecondAttempt { get; set; }
        public int DbThirdAttempt { get; set; }
        public bool IsStrike { get; set; }
        public bool IsSpare { get; set; }
        public bool IsSplit { get; set; }
        public int CurrentScore { get; set; }

    }
}
