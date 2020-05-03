using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace BowlingStats.Models
{
    public class GameModel
    {
        public int ID { get; set; }
        public int GameOrderID { get; set; }
        public int FinalScore { get; set; }
        public int FinalScoreHDP { get; set; }
        public ObservableCollection<FrameModel> Frames { get; set; }

        public List<string> allowedChars = new List<string>{ "F", "f", "X", "x", "-", "/" };

        public GameModel()
        {
            Frames = new ObservableCollection<FrameModel>();

            for (int i = 0; i < 10; i++)
            {
                //Frames[i] = new FrameModel();
                Frames.Add(new FrameModel());
            }
        }

        public Color TextColor
        {
            get
            {
                if (FinalScoreHDP < 200)
                    return Color.Black;
                else
                    if (FinalScoreHDP == 300)
                        return Color.DarkCyan;
                    else
                        return Color.Red;
            }
        }

        public string ScoresResume
        {
            get
            {
                return FinalScore + " (" + FinalScoreHDP + ")";
            }
        }

        internal string ValidateDetailScore()
        {
            int value;
            string message = string.Empty;

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    // First Attempt not null
                    if (string.IsNullOrEmpty(Frames[i].FirstAttempt))
                    {
                        message = $"Il primo tentativo del frame numero {i + 1} non contiene alcun punteggio!";
                        break;
                    }

                    // Valid chars
                    if (!string.IsNullOrEmpty(Frames[i].FirstAttempt) && !allowedChars.Contains(Frames[i].FirstAttempt) && !Int32.TryParse(Frames[i].FirstAttempt, out value))
                    {
                        message = $"Il frame numero {i + 1} contiene un carattere non valido nel primo tentativo!";
                        break;
                    }

                    if (!string.IsNullOrEmpty(Frames[i].SecondAttempt) && !allowedChars.Contains(Frames[i].SecondAttempt) && !Int32.TryParse(Frames[i].SecondAttempt, out value))
                    {
                        message = $"Il frame numero {i + 1} contiene un carattere non valido nel secondo tentativo!";
                        break;
                    }

                    if (!string.IsNullOrEmpty(Frames[i].ThirdAttempt) && !allowedChars.Contains(Frames[i].ThirdAttempt) && !Int32.TryParse(Frames[i].ThirdAttempt, out value) && i == 9)
                    {
                        message = $"Il frame numero {i + 1} contiene un carattere non valido nel terzo tentativo!";
                        break;
                    }

                    // Invalid situation in first attempt
                    if (Frames[i].FirstAttempt.Equals("/"))
                    {
                        message = $"Il frame numero {i + 1} contiene uno spare al primo tentativo!";
                        break;
                    }

                    // Invalid situation in second attempt
                    if (!string.IsNullOrEmpty(Frames[i].SecondAttempt) && Frames[i].SecondAttempt.Equals("X") && (i != 9 || (i==9 && !Frames[i].FirstAttempt.Equals("X") && !Frames[i].FirstAttempt.Equals("x"))))
                    {
                        message = $"Il frame numero {i + 1} contiene uno strike al secondo tentativo!";
                        break;
                    }

                    // Attempts dependencies
                    if ((Frames[i].FirstAttempt.Equals("X") || Frames[i].FirstAttempt.Equals("x")) && i != 9 && !string.IsNullOrEmpty(Frames[i].SecondAttempt))
                    {
                        message = $"Il frame numero {i + 1} contiene un punteggio nel secondo tentativo ma in caso di strike non è possibile!";
                        break;
                    }

                    if (!Frames[i].FirstAttempt.Equals("X") && !Frames[i].FirstAttempt.Equals("x") && string.IsNullOrEmpty(Frames[i].SecondAttempt))
                    {
                        message = $"Il frame numero {i + 1} non risulta completato!";
                        break;
                    }

                    if (i == 9)
                    {
                        if ((Frames[i].FirstAttempt.Equals("X") || Frames[i].FirstAttempt.Equals("x")) && Frames[i].SecondAttempt.Equals("/"))
                        {
                            message = $"Il frame numero {i + 1} contiene uno spare dopo lo strike!";
                            break;
                        }

                        if ((Frames[i].FirstAttempt.Equals("X") || Frames[i].FirstAttempt.Equals("x") || Frames[i].SecondAttempt.Equals("/")) && string.IsNullOrEmpty(Frames[i].ThirdAttempt))
                        {
                            message = $"Il frame numero {i + 1} non risulta completato!";
                            break;
                        }

                        if ((Frames[i].SecondAttempt.Equals("X") || Frames[i].SecondAttempt.Equals("x")) && Frames[i].ThirdAttempt.Equals("/"))
                        {
                            message = $"Il frame numero {i + 1} contiene un terzo tentativo non valido!";
                            break;
                        }
                    }

                    if (i < 9)
                    {
                        if (GetFrameScore(i) > 10)
                        {
                            message = $"Il frame numero {i + 1} contiene un punteggio troppo alto!";
                            break;
                        }
                    }
                    else
                    {
                        if (GetFrameScore(i) > 30)
                        {
                            message = $"Il frame numero {i + 1} contiene un punteggio troppo alto!";
                            break;
                        } else
                        {
                            if (GetFrameScore(i) > 20 && !((Frames[i].FirstAttempt.Equals("X") || Frames[i].FirstAttempt.Equals("x")) && (Frames[i].SecondAttempt.Equals("X") || Frames[i].SecondAttempt.Equals("x"))))
                            {
                                message = $"Il frame numero {i + 1} contiene un punteggio troppo alto!";
                                break;
                            } else
                            {
                                if (GetFrameScore(i) > 10 && !Frames[i].FirstAttempt.Equals("X") && !Frames[i].SecondAttempt.Equals("/")) {
                                    message = $"Il frame numero {i + 1} contiene un punteggio troppo alto!";
                                    break;
                                }
                            }
                        }
                    }
                }

                //if (string.IsNullOrEmpty(message))
                //{
                //    CalculateScore();
                //    message = $"Calculated = {FinalScore}";
                //}
            } catch (Exception e)
            {
                message = e.Message;
            }

            return message;
        }

        public string TournamentResume { get; set; }

        internal void CalculateScore()
        {
            for (int i = 0; i < 10; i++)
            {
                Frames[i].IsStrike = Frames[i].IsSpare = false;

                if (Frames[i].FirstAttempt.Equals("X") || Frames[i].FirstAttempt.Equals("x"))
                    Frames[i].IsStrike = true;
                else
                    if (Frames[i].SecondAttempt.Equals("/"))
                    Frames[i].IsSpare = true;

                if (i > 0)
                    Frames[i].CurrentScore = Frames[i - 1].CurrentScore;
                else
                    Frames[i].CurrentScore = 0;

                // aggiorno sempre il current secondo i punteggi del frame
                Frames[i].CurrentScore += GetFrameScore(i);

                if (Frames[i].IsSpare)
                {
                    if (i < 9) // frame 1-9
                        Frames[i].CurrentScore += GetAttemptScore(Frames[i + 1].FirstAttempt);
                }
                else
                {
                    if (Frames[i].IsStrike)
                    {
                        if (i < 8)
                        { // frame 1-8
                            if (GetAttemptScore(Frames[i + 1].FirstAttempt) == 10)
                                Frames[i].CurrentScore += 10 + GetAttemptScore(Frames[i + 2].FirstAttempt);
                            else
                                Frames[i].CurrentScore += GetFrameScore(i + 1);
                        }
                        else
                        {
                            if (i == 8) // frame 9
                            {
                                if (GetAttemptScore(Frames[i + 1].FirstAttempt) == 10)
                                    Frames[i].CurrentScore += 10 + GetAttemptScore(Frames[i + 1].SecondAttempt);
                                else
                                    if (Frames[i + 1].SecondAttempt.Equals("/"))
                                    Frames[i].CurrentScore += 10;
                                else
                                    Frames[i].CurrentScore += GetAttemptScore(Frames[i + 1].FirstAttempt) + GetAttemptScore(Frames[i + 1].SecondAttempt);
                            }
                        }
                    }
                }
            }

            FinalScore = Frames[9].CurrentScore;

        }

        private int GetFrameScore(int index)
        {
            if (Frames[index].FirstAttempt.Equals("X") || Frames[index].FirstAttempt.Equals("x") || Frames[index].SecondAttempt.Equals("/"))
                if (index < 9)
                    return 10;
                else
                {
                    if (Frames[index].FirstAttempt.Equals("X") || Frames[index].FirstAttempt.Equals("x"))
                    {
                        if (Frames[index].SecondAttempt.Equals("X") || Frames[index].SecondAttempt.Equals("x"))
                            return 20 + GetAttemptScore(Frames[index].ThirdAttempt);
                        else
                            if (Frames[index].ThirdAttempt.Equals("/"))
                            return 20;
                        else
                            return 10 + GetAttemptScore(Frames[index].SecondAttempt) + GetAttemptScore(Frames[index].ThirdAttempt);
                    } else
                    {
                        if (Frames[index].SecondAttempt.Equals("/"))
                            return 10 + GetAttemptScore(Frames[index].ThirdAttempt);
                    }
                }

                int first = Frames[index].FirstAttempt.Equals("-") || Frames[index].FirstAttempt.Equals("F") || Frames[index].FirstAttempt.Equals("f") ? 0 : Int32.Parse(Frames[index].FirstAttempt);
                int second = Frames[index].SecondAttempt.Equals("-") || Frames[index].SecondAttempt.Equals("F") || Frames[index].SecondAttempt.Equals("f") ? 0 : Int32.Parse(Frames[index].SecondAttempt);
                return first + second;
        }

        private int GetAttemptScore(string attempt)
        {
            if (attempt.Equals("X") || attempt.Equals("x"))
                return 10;

            if (attempt.Equals("F") || attempt.Equals("f") || attempt.Equals("-"))
                return 0;

            return Int32.Parse(attempt);
        }
    }
}
