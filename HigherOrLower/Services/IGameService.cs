using HigherOrLower.Dtos;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Wrappers;

namespace HigherOrLower.Services
{
    public interface IGameService
    {
        IResultWithStatus<GameInfoDto, string> CreateNewGame();

        IResultWithStatus<GameInfoWithGuessResultDto, string> TryEvaluateGuess(int gameDisplayId, string playerName, Guess guess);

        IResultWithStatus<GameInfoWithPlayersInfoDto, string> GetGameInfo(int gameDisplayId);
    }
}