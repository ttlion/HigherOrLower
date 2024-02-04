using HigherOrLower.Dtos;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Wrappers;

namespace HigherOrLower.Engines
{
    public interface IGameEngine
    {
        GameWithNextCardDto CreateNewGame();
        
        ResultWithStatus<GameWithNextCardAndGuessResultDto, EvaluateGuessStatus> TryDrawNextCardAndEvaluateGuess(int gameDisplayId, string playerName, Guess guess);
    }
}