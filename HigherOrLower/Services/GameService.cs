using HigherOrLower.Engines;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Extensions;
using HigherOrLower.Utils.Wrappers;

namespace HigherOrLower.Services
{
    public class GameService : IGameService
    {
        private readonly IGameEngine _gameEngine;

        public GameService(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public string CreateNewGame()
        {
            var resultWithStatus = _gameEngine.CreateNewGame();
            return ResultWithStatusToResultString(resultWithStatus, ErrorStatusToMessage(resultWithStatus.Status));
        }

        public string TryEvaluateGuess(int gameDisplayId, string playerName, Guess guess)
        {
            var resultWithStatus = _gameEngine.TryDrawNextCardAndEvaluateGuess(gameDisplayId, playerName, guess);
            return ResultWithStatusToResultString(resultWithStatus, ErrorStatusToMessage(resultWithStatus.Status, gameDisplayId, playerName));
        }

        public string GetGameInfo(int gameDisplayId)
        {
            var resultWithStatus = _gameEngine.TryGetGameInfo(gameDisplayId);
            return ResultWithStatusToResultString(resultWithStatus, ErrorStatusToMessage(resultWithStatus.Status, gameDisplayId));
        }

        private string ResultWithStatusToResultString<T,U>(IResultWithStatus<T, U> resultWithStatus, string errorMessage) where T : new()
        {
            return resultWithStatus.IsError ? errorMessage.ToJsonMessage() : resultWithStatus.Result.ToJson();

        }

        private static string ErrorStatusToMessage(CreateNewGameStatus createNewGameStatus)
        {
            return createNewGameStatus switch
            {
                _ => $"Error creating new game",
            };
        }

        private static string ErrorStatusToMessage(EvaluateGuessStatus evaluateGuessStatus, int gameDisplayId, string playerName)
        {
            return evaluateGuessStatus switch
            {
                EvaluateGuessStatus.ErrorGameDoesNotExist => $"Cannot make guess because game {gameDisplayId} does not exist",
                EvaluateGuessStatus.ErrorGameIsFinished => $"Cannot make guess because game {gameDisplayId} is already finished (check GameInfo endpoint)",
                EvaluateGuessStatus.ErrorCannotAddNewPlayers => $"Cannot make guess because cannot add new players to game {gameDisplayId} (check GameInfo endpoint)",
                EvaluateGuessStatus.ErrorAnotherPlayersTurn => $"Cannot make guess because it is not {playerName}'s turn in game {gameDisplayId} (check GameInfo endpoint)",
                EvaluateGuessStatus.ErrorNotProperPlayerToCloseTable => $"Cannot make guess because table {playerName} is already playing, but it is not the one that should close the table cycle to start the 2nd round of guesses (check GameInfo endpoint)",
                _ => $"Error making guess for player {playerName} in game {gameDisplayId} (check GameInfo endpoint)",
            };
        }

        private static string ErrorStatusToMessage(GetGameInfoStatus getPlayersScoresStatus, int gameDisplayId)
        {
            return getPlayersScoresStatus switch
            {
                GetGameInfoStatus.ErrorGameDoesNotExist => $"Cannot get game info because game {gameDisplayId} does not exist",
                _ => $"Error getting game's info for game {gameDisplayId}",
            };
        }
    }
}
