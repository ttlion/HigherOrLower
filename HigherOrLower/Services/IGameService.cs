using HigherOrLower.Utils.Enums;

namespace HigherOrLower.Services
{
    public interface IGameService
    {
        string CreateNewGame();

        public string TryEvaluateGuess(int gameDisplayId, string playerName, Guess guess);
    }
}