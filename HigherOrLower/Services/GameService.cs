using HigherOrLower.Dtos;
using HigherOrLower.Engines;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Extensions;

namespace HigherOrLower.Services
{
    public class GameService : IGameService
    {
        private static readonly string CreateNewGameSuccessMessageTemplate = "The first card from game {0} is {1}";
        private static readonly string EvaluateGuessResultMessageTemplate = "The next card in game {0} is {1}. {2}'s guess was {3}";

        private readonly IGameEngine _gameEngine;

        public GameService(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public string CreateNewGame()
        {
            var newGame = _gameEngine.CreateNewGame();
            return GetCreateNewGameResultMessage(newGame);
        }

        private string GetCreateNewGameResultMessage(GameWithNextCardDto newGame)
        {
            return string.Format(CreateNewGameSuccessMessageTemplate, newGame.Id, newGame.NextCard).ToJsonWithMessage();
        }

        public string TryEvaluateGuess(int gameDisplayId, string playerName, Guess guess)
        {
            var resultWithStatus = _gameEngine.TryDrawNextCardAndEvaluateGuess(gameDisplayId, playerName, guess);

            return resultWithStatus.IsError
                ? EvaluateGuessErrorStatusToString(resultWithStatus.Status, gameDisplayId, playerName)
                : GetEvaluateGuessResultMessage(playerName, resultWithStatus.Result)
            ;
        }

        private string EvaluateGuessErrorStatusToString(EvaluateGuessStatus evaluateGuessStatus, int gameDisplayId, string playerName)
        {
            var message = evaluateGuessStatus switch
            {
                EvaluateGuessStatus.ErrorGameDoesNotExist => $"Cannot make guess because game {gameDisplayId} does not exist",
                EvaluateGuessStatus.ErrorGameIsFinished => $"Cannot make guess because game {gameDisplayId} is already finished",
                EvaluateGuessStatus.ErrorCannotAddNewPlayers => $"Cannot make guess because cannot add new players to game {gameDisplayId}",
                EvaluateGuessStatus.ErrorAnotherPlayersTurn => $"Cannot make guess because it is not {playerName}'s turn in game {gameDisplayId}",
                EvaluateGuessStatus.ErrorNotProperPlayerToCloseTable => $"Cannot make guess because table {playerName} is already playing, but it is not the one that should close the table cycle to start the 2nd round of guesses (check player's endpoint)",
                _ => $"Error making guess for player {playerName} in game {gameDisplayId}",
            };

            return message.ToJsonWithMessage();
        }

        private string GetEvaluateGuessResultMessage(string playerName, GameWithNextCardAndGuessResultDto gameWithNextCardAndGuessResult)
        {
            return string.Format(EvaluateGuessResultMessageTemplate, gameWithNextCardAndGuessResult.Id, gameWithNextCardAndGuessResult.NextCard, playerName, gameWithNextCardAndGuessResult.GuessResult).ToJsonWithMessage();
        }
    }
}
