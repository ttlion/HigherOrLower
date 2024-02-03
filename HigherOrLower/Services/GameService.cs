using HigherOrLower.Engines;
using HigherOrLower.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HigherOrLower.Services
{
    public class GameService : IGameService
    {
        private static readonly string CreateNewGameSuccessMessageTemplate = "The first card from Game {0} is {1}";

        private readonly IGameEngine _gameEngine;

        public GameService(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public string? TryCreateNewGame()
        {
            var newGame = _gameEngine.TryCreateNewGame();
            return newGame != null ? string.Format(CreateNewGameSuccessMessageTemplate, newGame.Id, newGame.NextCard).ToJsonWithMessage() : null;
        }
    }
}
