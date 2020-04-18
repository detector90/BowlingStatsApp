﻿using BowlingStats.Entities;
using BowlingStats.Enums;
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
        }

        public Task<List<Tournament>> GetTournamentsAsync()
        {
            return _database.Table<Tournament>().OrderByDescending(x => x.EventDate).ToListAsync();
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
                var i = 0;

                if (game.ID != 0)
                {
                    _database.UpdateAsync(game);
                }
                else
                {
                    _database.InsertAsync(game);
                }
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public Task<int> DeleteGame(int id)
        {
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
