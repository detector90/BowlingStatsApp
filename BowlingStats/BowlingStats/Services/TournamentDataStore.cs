using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BowlingStats.Entities;
using BowlingStats.Enums;
using BowlingStats.Models;

namespace BowlingStats.Services
{
    public class TournamentDataStore : IDataStore<TournamentModel, GameModel, BowlingCenterModel, FrameModel>
    {
        List<TournamentModel> tournaments;
        List<GameModel> games;
        List<BowlingCenterModel> bowlingCenters;
        List<FrameModel> frames;

        public TournamentDataStore()
        {
            try
            {
                bowlingCenters = new List<BowlingCenterModel>();

                foreach (BowlingCenter dbBowlingCenter in App.Database.GetBowlingCentersAsync().Result)
                {
                    bowlingCenters.Add(new BowlingCenterModel
                    {
                        ID = dbBowlingCenter.ID,
                        City = dbBowlingCenter.City,
                        Name = dbBowlingCenter.Name,
                        Address = dbBowlingCenter.Address
                    });
                }

                tournaments = new List<TournamentModel>();

                foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
                {
                    ObservableCollection<GameModel> tournamentGames = new ObservableCollection<GameModel>();

                    foreach (Game dbGame in App.Database.GetTournamentGames(dbTournament.ID).Result)
                    {
                        tournamentGames.Add(new GameModel()
                        {
                            ID = dbGame.ID,
                            GameOrderID = dbGame.GameOrderID,
                            FinalScore = dbGame.Score,
                            FinalScoreHDP = dbGame.Score + dbTournament.Handicap <= 300 ? dbGame.Score + dbTournament.Handicap : 300
                        });
                    }

                    tournaments.Add(new TournamentModel
                    {
                        ID = dbTournament.ID,
                        Description = dbTournament.Description,
                        TournamentType = (Enums.TournamentTypeEnum)dbTournament.Type,
                        Handicap = dbTournament.Handicap,
                        EventDate = dbTournament.EventDate,
                        IsOfficial = dbTournament.IsOfficial,
                        Games = tournamentGames
                    }) ;
                }
            } catch (Exception e)
            {
                var a = e;
            }
        }

        public async Task<bool> AddTournamentAsync(TournamentModel tournament)
        {
            await App.Database.InsertTournamentAsync(
                new Tournament
                {
                    Type = (int)tournament.TournamentType,
                    BowlingCenterID = tournament.BowlingCenter.ID,
                    Handicap = tournament.Handicap,
                    Description = tournament.Description,
                    EventDate = tournament.EventDate,
                    IsOfficial = tournament.IsOfficial
                }
            );

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                var bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.ID).Result;

                tournaments.Add(new TournamentModel
                {
                    ID = dbTournament.ID,
                    Description = dbTournament.Description,
                    TournamentType = (Enums.TournamentTypeEnum)dbTournament.Type,
                    BowlingCenter = bowlingCenter != null ? new BowlingCenterModel
                    {
                        ID = bowlingCenter.ID,
                        Name = bowlingCenter.Name,
                        City = bowlingCenter.City,
                        Address = bowlingCenter.Address
                    } : null,
                    Handicap = dbTournament.Handicap,
                    EventDate = dbTournament.EventDate,
                    IsOfficial = dbTournament.IsOfficial
                });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateTournamentAsync(TournamentModel tournament, ICollection<GameModel> games)

        {
            await App.Database.UpdateTournamentAsync(
                new Tournament
                {
                    ID = tournament.ID,
                    Type = (int)tournament.TournamentType,
                    BowlingCenterID = tournament.BowlingCenter.ID,
                    Handicap = tournament.Handicap,
                    Description = tournament.Description,
                    EventDate = tournament.EventDate,
                    IsOfficial = tournament.IsOfficial
                }
            );

            if (games != null && games.Count > 0)
            {
                List<Game> gamesToUpdate = new List<Game>();

                foreach (GameModel game in games)
                {
                    gamesToUpdate.Add(new Game()
                    {
                        ID = game.ID,
                        GameOrderID = game.GameOrderID,
                        Score = game.FinalScore,
                        TournamentID = tournament.ID
                    });
                }

                await App.Database.UpdateGamesAsync(gamesToUpdate);
            }

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                var bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.ID).Result;

                tournaments.Add(new TournamentModel
                {
                    ID = dbTournament.ID,
                    Description = dbTournament.Description,
                    TournamentType = (Enums.TournamentTypeEnum)dbTournament.Type,
                    BowlingCenter = bowlingCenter != null ? new BowlingCenterModel
                    {
                        ID = bowlingCenter.ID,
                        Name = bowlingCenter.Name,
                        City = bowlingCenter.City,
                        Address = bowlingCenter.Address
                    } : null,
                    Handicap = dbTournament.Handicap,
                    EventDate = dbTournament.EventDate,
                    IsOfficial = dbTournament.IsOfficial
                });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteTournamentAsync(int id)
        {
            int deletedGames = App.Database.DeleteTournamentGames(id).Result;

            await App.Database.DeleteTournamentAsync(id);

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                var bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.ID).Result;

                tournaments.Add(new TournamentModel
                {
                    ID = dbTournament.ID,
                    Description = dbTournament.Description,
                    TournamentType = (Enums.TournamentTypeEnum)dbTournament.Type,
                    BowlingCenter = bowlingCenter != null ? new BowlingCenterModel
                    {
                        ID = bowlingCenter.ID,
                        Name = bowlingCenter.Name,
                        City = bowlingCenter.City,
                        Address = bowlingCenter.Address
                    } : null,
                    Handicap = dbTournament.Handicap,
                    EventDate = dbTournament.EventDate,
                    IsOfficial = dbTournament.IsOfficial
                });
            }

            return await Task.FromResult(true);
        }

        public async Task<TournamentModel> GetTournamentAsync(int id)
        {
            return await Task.FromResult(tournaments.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<TournamentModel>> GetTournamentsAsync(bool forceRefresh = false)
        {
            tournaments.Clear();

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                var bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.BowlingCenterID).Result;

                TournamentModel tournament = new TournamentModel
                {
                    ID = dbTournament.ID,
                    Description = dbTournament.Description,
                    TournamentType = (Enums.TournamentTypeEnum)dbTournament.Type,
                    BowlingCenter = bowlingCenter != null ? new BowlingCenterModel
                    {
                        ID = bowlingCenter.ID,
                        Name = bowlingCenter.Name,
                        City = bowlingCenter.City,
                        Address = bowlingCenter.Address
                    } : null,
                    Handicap = dbTournament.Handicap,
                    EventDate = dbTournament.EventDate,
                    IsOfficial = dbTournament.IsOfficial,
                    Games = new ObservableCollection<GameModel>()
                };

                foreach (Game dbgame in App.Database.GetTournamentGames(dbTournament.ID).Result)
                {
                    tournament.Games.Add(new GameModel()
                    {
                        ID = dbgame.ID,
                        GameOrderID = dbgame.GameOrderID,
                        FinalScore = dbgame.Score,
                        FinalScoreHDP = dbgame.Score + dbTournament.Handicap <= 300 ? dbgame.Score + dbTournament.Handicap : 300
                    });
                }

                tournaments.Add(tournament);
            }

            return tournaments.AsEnumerable();
        }

        public bool SaveTournamentGame(GameModel game, int tournamentId)
        {
            bool result;

            Game gameTodb = new Game()
            {
                TournamentID = tournamentId,
                ID = game.ID,
                GameOrderID = game.GameOrderID,
                Score = game.CalculateScore()
            };

            result = App.Database.UpdateGame(gameTodb);

            if (result)
            {
                result = SaveGameFrames(gameTodb.ID, game.Frames);
            }

            return result;
        }

        public bool SaveGameFrames(int gameID, ObservableCollection<FrameModel> frames)
        {
            for (int i = 0; i < 10; i++)
            {
                Frame frameToDB = new Frame()
                {
                    ID = frames[i].ID,
                    GameID = gameID,
                    FrameOrderID = i,
                    FirstAttempt = frames[i].DbFirstAttempt,
                    SecondAttempt = frames[i].DbSecondAttempt,
                    ThirdAttempt = frames[i].DbThirdAttempt,
                    IsSpare = frames[i].IsSpare,
                    IsStrike = frames[i].IsStrike
                };

                App.Database.UpdateFrame(frameToDB);
            }

            return true;
        }

        public bool DeleteTournamentGame(int gameId)
        {
            if (App.Database.DeleteGame(gameId).Result == 1)
                return true;
            else
                return false;
        }

        public Task<IEnumerable<GameModel>> GetTournamentGamesAsync(int tournamentId)
        {
            games = new List<GameModel>();
            Tournament dbTournament = App.Database.GetTournamentsAsync(App.Filter).Result.Where(x => x.ID == tournamentId).FirstOrDefault();

            foreach (Game dbTournamentGame in App.Database.GetTournamentGames(tournamentId).Result)
            {
                var gameFrames = GetFrameModels(App.Database.GetGameFrames(dbTournamentGame.ID).Result);

                games.Add(new GameModel
                {
                    ID = dbTournamentGame.ID,
                    GameOrderID = dbTournamentGame.GameOrderID,
                    FinalScore = dbTournamentGame.Score,
                    FinalScoreHDP = dbTournamentGame.Score + dbTournament.Handicap <= 300 ? dbTournamentGame.Score + dbTournament.Handicap : 300,
                    Frames = gameFrames,
                });
            }

            return Task.FromResult(games.AsEnumerable());
        }

        private ObservableCollection<FrameModel> GetFrameModels(List<Frame> dbFrames)
        {
            ObservableCollection<FrameModel> frames = new ObservableCollection<FrameModel>();

            if (dbFrames.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    FrameModel frame = new FrameModel()
                    {
                        ID = dbFrames[i].ID,
                        DbFirstAttempt = dbFrames[i].FirstAttempt,
                        DbSecondAttempt = dbFrames[i].SecondAttempt,
                        DbThirdAttempt = dbFrames[i].ThirdAttempt,
                        IsSpare = dbFrames[i].IsSpare,
                        IsStrike = dbFrames[i].IsStrike
                    };

                    if (frame.IsStrike)
                    {
                        frame.FirstAttempt = "X";

                        if (i == 9)
                        {
                            if (frame.DbSecondAttempt == 10)
                            {
                                frame.SecondAttempt = "X";

                                if (frame.DbThirdAttempt == 10)
                                    frame.ThirdAttempt = "X";
                                else
                                    frame.ThirdAttempt = frame.DbThirdAttempt.ToString();
                            }
                            else
                                if (frame.DbSecondAttempt + frame.DbThirdAttempt == 10)
                                {
                                    frame.SecondAttempt = frame.DbSecondAttempt.ToString();
                                    frame.ThirdAttempt = "/";
                                } else
                            {
                                frame.SecondAttempt = frame.DbSecondAttempt.ToString();
                                frame.ThirdAttempt = frame.DbThirdAttempt.ToString();
                            }
                        }
                            
                    } else 
                        if (frame.IsSpare)
                    {
                        frame.FirstAttempt = frame.DbFirstAttempt.ToString();
                        frame.SecondAttempt = "/";

                        if (i == 9)
                        {
                            if (frame.DbThirdAttempt == 10)
                                frame.ThirdAttempt = "X";
                            else
                                frame.ThirdAttempt = frame.DbThirdAttempt.ToString();
                        }

                        
                    } else
                    {
                        frame.FirstAttempt = frame.DbFirstAttempt.ToString();
                        frame.SecondAttempt = frame.DbSecondAttempt.ToString();
                    }

                    frames.Add(frame);
                }
            }

            return frames;
        }

        public Task<IEnumerable<GameModel>> GetAllGames(OfficialFilterEnum filter)
        {
            games = new List<GameModel>();

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                if (filter == OfficialFilterEnum.All || (filter == OfficialFilterEnum.OnlyOfficial && dbTournament.IsOfficial) || (filter == OfficialFilterEnum.OnlyUnofficial && !dbTournament.IsOfficial)) {
                    BowlingCenter bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.BowlingCenterID).Result;

                    foreach (Game dbGame in App.Database.GetTournamentGames(dbTournament.ID).Result)
                    {
                        games.Add(new GameModel()
                        {
                            ID = dbGame.ID,
                            TournamentResume = bowlingCenter != null ? string.Concat(bowlingCenter.City, " - ", dbTournament.EventDate.ToString("dd/MM/yyyy")) : dbTournament.EventDate.ToString("dd/MM/yyyy"),
                            GameOrderID = dbGame.GameOrderID,
                            FinalScore = dbGame.Score,
                            FinalScoreHDP = dbGame.Score + dbTournament.Handicap <= 300 ? dbGame.Score + dbTournament.Handicap : 300
                        });
                    }
                }
            }

            //foreach (Game dbGame in App.Database.GetAllGames().Result)
            //{
            //    Tournament tournament = App.Database.GetTournamentAsync(dbGame.TournamentID).Result;
            //    BowlingCenter bowlingCenter = tournament.BowlingCenterID != 0 ? App.Database.GetBowlingCenterAsync(tournament.BowlingCenterID).Result : null;

            //    if (filter == OfficialFilterEnum.All || (filter == OfficialFilterEnum.OnlyOfficial && tournament.IsOfficial) || (filter == OfficialFilterEnum.OnlyUnofficial && !tournament.IsOfficial))
            //    {
            //        games.Add(new GameModel()
            //        {
            //            ID = dbGame.ID,
            //            TournamentResume = bowlingCenter != null ? string.Concat(bowlingCenter.City, " - ", tournament.EventDate.ToString("dd/MM/yyyy")) : tournament.EventDate.ToString("dd/MM/yyyy"),
            //            GameOrderID = dbGame.GameOrderID,
            //            FinalScore = dbGame.Score,
            //            FinalScoreHDP = dbGame.Score + tournament.Handicap <= 300 ? dbGame.Score + tournament.Handicap : 300
            //        });
            //    }
            //}
            return Task.FromResult(games.AsEnumerable());
        }

        public Task<IEnumerable<FrameModel>> GetAllFrames(OfficialFilterEnum filter)
        {
            frames = new List<FrameModel>();

            foreach (Tournament dbTournament in App.Database.GetTournamentsAsync(App.Filter).Result)
            {
                if (filter == OfficialFilterEnum.All || (filter == OfficialFilterEnum.OnlyOfficial && dbTournament.IsOfficial) || (filter == OfficialFilterEnum.OnlyUnofficial && !dbTournament.IsOfficial))
                {
                    BowlingCenter bowlingCenter = App.Database.GetBowlingCenterAsync(dbTournament.BowlingCenterID).Result;

                    foreach (Game dbGame in App.Database.GetTournamentGames(dbTournament.ID).Result)
                    {
                        var framesToAdd = App.Database.GetGameFrames(dbGame.ID).Result;

                        framesToAdd.ForEach(x => frames.Add(new FrameModel()
                        {
                            ID = x.ID,
                            DbFirstAttempt = x.FirstAttempt,
                            DbSecondAttempt = x.SecondAttempt,
                            DbThirdAttempt = x.ThirdAttempt,
                            IsSpare = x.IsSpare,
                            IsStrike = x.IsStrike
                        }));
                    }
                }
            }


            //foreach (Game dbGame in App.Database.GetAllGames().Result)
            //{
            //    Tournament tournament = App.Database.GetTournamentAsync(dbGame.TournamentID).Result;
            //    BowlingCenter bowlingCenter = tournament.BowlingCenterID != 0 ? App.Database.GetBowlingCenterAsync(tournament.BowlingCenterID).Result : null;

            //    if (filter == OfficialFilterEnum.All || (filter == OfficialFilterEnum.OnlyOfficial && tournament.IsOfficial) || (filter == OfficialFilterEnum.OnlyUnofficial && !tournament.IsOfficial))
            //    {
            //        var framesToAdd = App.Database.GetGameFrames(dbGame.ID).Result;

            //        framesToAdd.ForEach(x => frames.Add(new FrameModel()
            //        {
            //            ID = x.ID,
            //            DbFirstAttempt = x.FirstAttempt,
            //            DbSecondAttempt = x.SecondAttempt,
            //            DbThirdAttempt = x.ThirdAttempt,
            //            IsSpare = x.IsSpare,
            //            IsStrike = x.IsStrike
            //        }));

            //        //games.Add(new GameModel()
            //        //{
            //        //    ID = dbGame.ID,
            //        //    TournamentResume = bowlingCenter != null ? string.Concat(bowlingCenter.City, " - ", tournament.EventDate.ToString("dd/MM/yyyy")) : tournament.EventDate.ToString("dd/MM/yyyy"),
            //        //    GameOrderID = dbGame.GameOrderID,
            //        //    FinalScore = dbGame.Score,
            //        //    FinalScoreHDP = dbGame.Score + tournament.Handicap <= 300 ? dbGame.Score + tournament.Handicap : 300
            //        //});
            //    }
            //}
            return Task.FromResult(frames.AsEnumerable());
        }

        public async Task<bool> AddBowlingCenterAsync(BowlingCenterModel item)
        {
            await App.Database.InsertBowlingCenterAsync(
                new BowlingCenter
                {
                    City = item.City,
                    Name = item.Name,
                    Address = item.Address
                }
            );

            foreach (BowlingCenter dbBowlingCenter in App.Database.GetBowlingCentersAsync().Result)
            {
                bowlingCenters.Add(new BowlingCenterModel
                {
                    ID = dbBowlingCenter.ID,
                    City = dbBowlingCenter.City,
                    Name = dbBowlingCenter.Name,
                    Address = dbBowlingCenter.Address
                });
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateBowlingCenterAsync(BowlingCenterModel bowlingCenter)
        {
            await App.Database.UpdateBowlingCenterAsync(
                new BowlingCenter
                {
                    ID = bowlingCenter.ID,
                    City = bowlingCenter.City,
                    Name = bowlingCenter.Name,
                    Address = bowlingCenter.Address
                }
            );

            foreach (BowlingCenter dbBowlingCenter in App.Database.GetBowlingCentersAsync().Result)
            {
                bowlingCenters.Add(new BowlingCenterModel
                {
                    ID = dbBowlingCenter.ID,
                    City = dbBowlingCenter.City,
                    Name = dbBowlingCenter.Name,
                    Address = dbBowlingCenter.Address
                });
            }

            return await Task.FromResult(true);
        }

        

        public async Task<bool> DeleteBowlingCenterAsync(int id)
        {
            // TODO: Aggiungere controllo che non ci siano tornei associati

            await App.Database.DeleteBowlingCenterAsync(id);

            foreach (BowlingCenter dbBowlingCenter in App.Database.GetBowlingCentersAsync().Result)
            {
                bowlingCenters.Add(new BowlingCenterModel
                {
                    ID = dbBowlingCenter.ID,
                    City = dbBowlingCenter.City,
                    Name = dbBowlingCenter.Name,
                    Address = dbBowlingCenter.Address
                });
            }

            return await Task.FromResult(true);
        }

        public async Task<BowlingCenterModel> GetBowlingCenterAsync(int id)
        {
            return await Task.FromResult(bowlingCenters.FirstOrDefault(s => s.ID == id));
        }

        public async Task<IEnumerable<BowlingCenterModel>> GetBowlingCentersAsync(bool forceRefresh = false)
        {
            bowlingCenters.Clear();

            foreach (BowlingCenter dbBowlingCenter in App.Database.GetBowlingCentersAsync().Result)
            {
                bowlingCenters.Add(new BowlingCenterModel
                {
                    ID = dbBowlingCenter.ID,
                    City = dbBowlingCenter.City,
                    Name = dbBowlingCenter.Name,
                    Address = dbBowlingCenter.Address
                });
            }

            return bowlingCenters.AsEnumerable();
        }
    }
}