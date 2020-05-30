using BowlingStats.Entities;
using BowlingStats.Enums;
using BowlingStats.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BowlingStats.Utils
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Tournament>().Wait();
            _database.CreateTableAsync<Game>().Wait();
            _database.CreateTableAsync<BowlingCenter>().Wait();
            _database.CreateTableAsync<Frame>().Wait();
        }

        public Task<List<Tournament>> GetTournamentsAsync(Filters globalFilter)
        {
            var tournaments = _database.Table<Tournament>().Where(x => x.EventDate < globalFilter.DateTo && x.EventDate > globalFilter.DateFrom);

            if (globalFilter.BowlingID != 0)
                tournaments = tournaments.Where(x => x.BowlingCenterID == globalFilter.BowlingID);
                
                
            return tournaments.OrderByDescending(x => x.EventDate).ToListAsync();
        }

        public Task<Tournament> GetTournamentAsync(int tournamentID)
        {
            return _database.Table<Tournament>().Where(x => x.ID == tournamentID).FirstOrDefaultAsync();
        }

        public Task<int> InsertTournamentAsync(Tournament tournament)
        {
            return _database.InsertAsync(tournament);
        }

        public Task<int> UpdateTournamentAsync(Tournament tournament)
        {
            return _database.UpdateAsync(tournament);
        }

        public Task<int> DeleteTournamentAsync(int id)
        {
            return _database.DeleteAsync<Tournament>(id);
        }

        public Task<int> UpdateGamesAsync(ICollection<Game> games)
        {
            foreach (Game game in games)
            {
                if (game.ID != 0)
                {
                    _database.UpdateAsync(game);
                } else
                {
                    _database.InsertAsync(game);
                }
            }

            return _database.UpdateAllAsync(games);
        }

        public bool UpdateGame(Game game)
        {
            try
            {
                if (game.ID != 0)
                {
                    int i = _database.UpdateAsync(game).Result;
                }
                else
                {
                    int i = _database.InsertAsync(game).Result;
                }
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public Task<int> DeleteGame(int id)
        {
            _database.Table<Frame>().DeleteAsync(x => x.GameID == id);

            return _database.Table<Game>().DeleteAsync(x => x.ID == id);
        }

        public Task<int> DeleteTournamentGames(int tournamentId)
        {
            return _database.Table<Game>().DeleteAsync(x => x.TournamentID == tournamentId);
        }

        public Task<List<Game>> GetTournamentGames(int tournamentId)
        {
            return _database.Table<Game>().Where(x => x.TournamentID == tournamentId).OrderBy(x => x.GameOrderID).ToListAsync();
        }

        public Task<List<Game>> GetAllGames()
        {
            return _database.Table<Game>().ToListAsync();
        }

        public Task<List<Frame>> GetAllFrames()
        {
            //return _database.Table<Frame>().Where(x => x.GameID != 0).ToListAsync();
            return _database.Table<Frame>().ToListAsync();
        }

        public Task<List<Frame>> GetGameFrames(int gameId)
        {
            return _database.Table<Frame>().Where(x => x.GameID == gameId).OrderBy(x => x.FrameOrderID).ToListAsync();
        }

        public bool UpdateFrame(Frame frame)
        {
            try
            {
                var i = 0;

                if (frame.ID != 0)
                {
                    _database.UpdateAsync(frame);
                }
                else
                {
                    _database.InsertAsync(frame);
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public Task<int> DeleteFrames(int gameId)
        {
            return _database.Table<Frame>().DeleteAsync(x => x.GameID == gameId);
        }

        #region BowlingCenter
        public Task<List<BowlingCenter>> GetBowlingCentersAsync()
        {
            return _database.Table<BowlingCenter>().OrderBy(x => x.City).ThenBy(x => x.Name).ToListAsync();
        }

        public Task<BowlingCenter> GetBowlingCenterAsync(int bowlingCenterId)
        {
            return _database.Table<BowlingCenter>().Where(x => x.ID == bowlingCenterId).FirstOrDefaultAsync();
        }

        public Task<int> InsertBowlingCenterAsync(BowlingCenter bowlingCenter)
        {
            return _database.InsertAsync(bowlingCenter);
        }

        public Task<int> UpdateBowlingCenterAsync(BowlingCenter bowlingCenter)
        {
            return _database.UpdateAsync(bowlingCenter);
        }

        public Task<int> DeleteBowlingCenterAsync(int id)
        {
            return _database.DeleteAsync<BowlingCenter>(id);
        }
        #endregion
    }
}
