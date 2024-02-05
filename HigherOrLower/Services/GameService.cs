using HigherOrLower.Dtos;
using HigherOrLower.Engines;
using HigherOrLower.Utils.Enums;
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

        public IResultWithStatus<GameInfoDto, string> CreateNewGame()
        {
            var resultWithStatus = _gameEngine.CreateNewGame();
            return ResultWithEnumStatusToResultWithString(resultWithStatus, "Success creating new game", ErrorStatusToMessage(resultWithStatus.Status));
        }

        public IResultWithStatus<GameInfoWithGuessResultDto, string> TryEvaluateGuess(int gameDisplayId, string playerName, Guess guess)
        {
            var resultWithStatus = _gameEngine.TryDrawNextCardAndEvaluateGuess(gameDisplayId, playerName, guess);
            return ResultWithEnumStatusToResultWithString(resultWithStatus, "Success making guess", ErrorStatusToMessage(resultWithStatus.Status, gameDisplayId, playerName));
        }

        public IResultWithStatus<GameInfoWithPlayersInfoDto, string> GetGameInfo(int gameDisplayId)
        {
            var resultWithStatus = _gameEngine.TryGetGameInfo(gameDisplayId);
            return ResultWithEnumStatusToResultWithString(resultWithStatus, "Success getting game info", ErrorStatusToMessage(resultWithStatus.Status, gameDisplayId));
        }

        private IResultWithStatus<T, string> ResultWithEnumStatusToResultWithString<T,U>(IResultWithStatus<T, U> resultWithStatus, string successMessage, string errorMessage) where T : new()
        {
            return resultWithStatus.IsError
                ? ResultWithStatus<T, string>.Error(errorMessage)
                : ResultWithStatus<T, string>.Success(resultWithStatus.Result, successMessage);
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
