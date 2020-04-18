using BowlingStats.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BowlingStats.Services
{
    public interface IDataStore<T,Q,Y>
    {
        Task<bool> AddTournamentAsync(T item);
        Task<bool> UpdateTournamentAsync(T item, ICollection<Q> games);
        //Task<bool> UpdateTournamentAsync(T item);

        Task<bool> DeleteTournamentAsync(int id);
        Task<T> GetTournamentAsync(int id);
        Task<IEnumerable<T>> GetTournamentsAsync(bool forceRefresh = false);
        bool SaveTournamentGame(Q game, int tournamentId);
        bool DeleteTournamentGame(int gameId);
        Task<IEnumerable<Q>> GetTournamentGamesAsync(int tournamentId);

        #region Statistics
        Task<IEnumerable<Q>> GetAllGames(OfficialFilterEnum filter);
        //Task<IEnumerable<Q>> GetAllGames();
        #endregion

        #region BowlingCenters
        Task<bool> AddBowlingCenterAsync(Y item);
        //bool AddBowlingCenter(Y item);

        Task<bool> UpdateBowlingCenterAsync(Y item);
        //Task<bool> UpdateTournamentAsync(T item);

        Task<bool> DeleteBowlingCenterAsync(int id);
        Task<Y> GetBowlingCenterAsync(int id);
        Task<IEnumerable<Y>> GetBowlingCentersAsync(bool forceRefresh = false);
        #endregion
    }
}
