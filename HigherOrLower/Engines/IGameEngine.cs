using HigherOrLower.Dtos;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Wrappers;

namespace HigherOrLower.Engines
{
    public interface IGameEngine
    {
        IResultWithStatus<GameInfoDto, CreateNewGameStatus> CreateNewGame();

        IResultWithStatus<GameInfoWithGuessResultDto, EvaluateGuessStatus> TryDrawNextCardAndEvaluateGuess(int gameDisplayId, string playerName, Guess guess);

        IResultWithStatus<GameInfoWithPlayersInfoDto, GetGameInfoStatus> TryGetGameInfo(int gameDisplayId);
    }
}