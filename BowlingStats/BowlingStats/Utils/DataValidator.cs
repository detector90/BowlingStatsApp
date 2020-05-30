using BowlingStats.Entities;
using BowlingStats.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BowlingStats.Utils
{
    public static class DataValidator
    {
        public static ValidationResult Validate(GameModel game, int tournamentID)
        {
            if (game.GameOrderID <= 0)
            {
                return new ValidationResult("Attenzione, il progressivo partita deve essere un numero positivo e diverso da 0!");
            }

            List<Game> gamesList = App.Database.GetTournamentGames(tournamentID).Result.Where(x => x.GameOrderID == game.GameOrderID && x.ID != game.ID).ToList();

            if (gamesList.Count > 0)
            {
                return new ValidationResult("Attenzione, il progressivo partita esiste già per questo evento!");
            }

            if (game.FinalScore < 0 || game.FinalScore > 300)
            {
                return new ValidationResult("Attenzione, il punteggio della partita deve essere compreso tra 0 e 300!");
            }

            string message;

            if (!string.IsNullOrEmpty(message = game.ValidateDetailScore()))
            {
                return new ValidationResult(message);
            }

            return new ValidationResult(string.Empty);
        }

        public static ValidationResult Validate(TournamentModel tournament)
        {
            if (tournament.BowlingCenter == null)
            {
                return new ValidationResult("Attenzione, seleziona un centro bowling. Se non il centro non è presente nella lista, inseriscilo nell'anagrafica corrispondente!");
            }

            if (string.IsNullOrEmpty(tournament.Description))
            {
                return new ValidationResult("Attenzione, inserisci una descrizione del torneo!");
            }

            return new ValidationResult(string.Empty);
        }

        public static ValidationResult Validate(BowlingCenterModel bowlingCenter)
        {
            if (string.IsNullOrEmpty(bowlingCenter.City))
            {
                return new ValidationResult("Attenzione, inserisci la città del centro bowling!");
            }

            if (string.IsNullOrEmpty(bowlingCenter.Name))
            {
                return new ValidationResult("Attenzione, inserisci il nome del centro bowling!");
            }

            return new ValidationResult(string.Empty);
        }
    }
}
