using BowlingStats.Models;
using BowlingStats.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BowlingStats.ViewModels
{
    public class GameDetailViewModel : BaseViewModel
    {
        //public GameModel Game { get; set; }
        private GameModel game;
        public GameModel Game
        {
            set
            {
                if (game != value)
                {
                    game = value;
                    OnPropertyChanged("Game");
                }
            }
            get
            {
                return game;
            }
        }

        public int CurrentFrame { get; set; }
        public int CurrentAttempt { get; set; }


        public ObservableCollection<FrameModel> Frames
        {
            set
            {
                if (game.Frames != value)
                {
                    game.Frames = value;
                    OnPropertyChanged("Frames");
                }
            }
            get
            {
                return game.Frames;
            }
        }

        public void SetSelected(bool selected, int index = -1)
        {
            if (index >= 0)
            {
                if (CurrentAttempt == 1)
                    Frames[index].FirstAttemptSelected = selected;
                if (CurrentAttempt == 2)
                    Frames[index].SecondAttemptSelected = selected;
                if (CurrentAttempt == 3)
                    Frames[index].ThirdAttemptSelected = selected;
            } else
            {
                Frames[CurrentFrame].FirstAttemptSelected = selected;
                Frames[CurrentFrame].SecondAttemptSelected = selected;
                Frames[CurrentFrame].ThirdAttemptSelected = selected;
            }

            OnPropertyChanged("Frames");
        }

        public int SetFramePointAndReturnRemainingPins(string value)
        {
            int remainingPins = 10;

            SetSelected(false, CurrentFrame);
            if (CurrentAttempt == 1)
            {
                if (value.Equals("10"))
                {
                    Frames[CurrentFrame].FirstAttempt = "X";
                    Frames[CurrentFrame].DbFirstAttempt = 10;

                    Frames[CurrentFrame].SecondAttempt = "";
                    Frames[CurrentFrame].DbSecondAttempt = 0;


                    if (CurrentFrame < 9)
                    {
                        CurrentFrame++;
                        CurrentAttempt = 1;
                    } else
                    {
                        Frames[CurrentFrame].ThirdAttempt = "";
                        Frames[CurrentFrame].DbThirdAttempt = 0;

                        CurrentAttempt = 2;
                    }
                }
                else
                {
                    if (value.Equals("0"))
                    {
                        Frames[CurrentFrame].FirstAttempt = "-";
                        Frames[CurrentFrame].DbFirstAttempt = 0;
                    }
                    else
                    {
                        Frames[CurrentFrame].FirstAttempt = value;
                        Frames[CurrentFrame].DbFirstAttempt = Int32.Parse(value);
                        remainingPins = 10 - Int32.Parse(value);
                    }
                    CurrentAttempt = 2;
                }
            } else
            {
                if (CurrentAttempt == 2)
                {
                    if (CurrentFrame < 9)
                    {
                        if (value.Equals((10 - Frames[CurrentFrame].DbFirstAttempt).ToString()))
                        {
                            Frames[CurrentFrame].SecondAttempt = "/";
                            Frames[CurrentFrame].DbSecondAttempt = Int32.Parse(value);
                        }
                        else
                        {
                            if (value.Equals("0"))
                            {
                                Frames[CurrentFrame].SecondAttempt = "-";
                                Frames[CurrentFrame].DbSecondAttempt = 0;
                            }
                            else
                            {
                                Frames[CurrentFrame].SecondAttempt = value;
                                Frames[CurrentFrame].DbSecondAttempt = Int32.Parse(value);
                            }
                        }

                        CurrentFrame++;
                        CurrentAttempt = 1;
                        remainingPins = 10;
                    } else
                    {
                        if (Frames[CurrentFrame].FirstAttempt.Equals("X"))
                        {
                            if (value.Equals("10"))
                            {
                                Frames[CurrentFrame].SecondAttempt = "X";
                                Frames[CurrentFrame].DbSecondAttempt = 10;

                                Frames[CurrentFrame].ThirdAttempt = "";
                                Frames[CurrentFrame].DbThirdAttempt = 0;
                            } else
                            {
                                if (value.Equals("0"))
                                {
                                    Frames[CurrentFrame].SecondAttempt = "-";
                                    Frames[CurrentFrame].DbSecondAttempt = 0;
                                }
                                else
                                {
                                    Frames[CurrentFrame].SecondAttempt = value;
                                    Frames[CurrentFrame].DbSecondAttempt = Int32.Parse(value);
                                    remainingPins = 10 - Int32.Parse(value);
                                }
                            }

                            CurrentAttempt++;
                        } else
                        {
                            if (value.Equals((10 - Frames[CurrentFrame].DbFirstAttempt).ToString()))
                            {
                                Frames[CurrentFrame].SecondAttempt = "/";
                                Frames[CurrentFrame].DbSecondAttempt = Int32.Parse(value);

                                Frames[CurrentFrame].ThirdAttempt = "";
                                Frames[CurrentFrame].DbThirdAttempt = 0;
                                CurrentAttempt++;
                            } else
                            {
                                if (value.Equals("0"))
                                {
                                    Frames[CurrentFrame].SecondAttempt = "-";
                                    Frames[CurrentFrame].DbSecondAttempt = 0;
                                }
                                else
                                {
                                    Frames[CurrentFrame].SecondAttempt = value;
                                    Frames[CurrentFrame].DbSecondAttempt = Int32.Parse(value);
                                    CurrentFrame = 0;
                                    CurrentAttempt = 1;
                                }
                            }
                        }
                    }

                } else
                {
                    if (Frames[CurrentFrame].SecondAttempt.Equals("X") || Frames[CurrentFrame].SecondAttempt.Equals("/"))
                    {
                        if (value.Equals("10"))
                        {
                            Frames[CurrentFrame].ThirdAttempt = "X";
                            Frames[CurrentFrame].DbThirdAttempt = 10;
                        }
                        else
                        {
                            if (value.Equals("0"))
                            {
                                Frames[CurrentFrame].ThirdAttempt = "-";
                                Frames[CurrentFrame].DbThirdAttempt = 0;
                            }
                            else
                            {
                                Frames[CurrentFrame].ThirdAttempt = value;
                                Frames[CurrentFrame].DbThirdAttempt = Int32.Parse(value);
                                
                            }
                        }
                    } else
                    {
                        if (value.Equals((10 - Frames[CurrentFrame].DbSecondAttempt).ToString()))
                        {
                            Frames[CurrentFrame].ThirdAttempt = "/";
                            Frames[CurrentFrame].DbThirdAttempt = Int32.Parse(value);
                        }
                        else
                        {
                            if (value.Equals("0"))
                            {
                                Frames[CurrentFrame].ThirdAttempt = "-";
                                Frames[CurrentFrame].DbThirdAttempt = 0;
                            }
                            else
                            {
                                Frames[CurrentFrame].ThirdAttempt = value;
                                Frames[CurrentFrame].DbThirdAttempt = Int32.Parse(value);
                            }
                        }
                    }

                    CurrentFrame = 0;
                    CurrentAttempt = 1;
                }
            }
            
            SetSelected(true, CurrentFrame);

            return remainingPins;
        }

        public string Title { get; set; }
        public int TournamentID { get; set; }
        public Command ButtonClickedCommand { get; set; }
        public Command FrameSelectedCommand { get; set; }

        public GameDetailViewModel(int tournamentID)
        {
            Title = "Nuova partita";

            Game = new GameModel();
            Game.GameOrderID = BusinessLogic.ProposedGameOrderID(tournamentID);
            TournamentID = tournamentID;

            CurrentFrame = 0;
            CurrentAttempt = 1;
            SetSelected(true, CurrentFrame);
        }

        public GameDetailViewModel(GameModel game, int tournamentID)
        {
            Title = "Dettaglio partita";

            Game = game;
            TournamentID = tournamentID;

            CurrentFrame = 0;
            CurrentAttempt = 1;
            SetSelected(true, CurrentFrame);
        }
    }
}
