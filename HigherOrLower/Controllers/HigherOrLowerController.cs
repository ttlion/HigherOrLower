using HigherOrLower.Services;
using HigherOrLower.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HigherOrLower.Controllers
{
    [ApiController]
    public class HigherOrLowerController : ControllerBase
    {
        private readonly IGameService _gameService;
            
        public HigherOrLowerController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [Route("NewGame")]
        [HttpPost]
        public ActionResult NewGame()
        {
            return Ok(_gameService.CreateNewGame());
        }

        [Route("Game/{gameId}/CurrentCard")]
        [HttpGet]
        public void CurrentCard(int gameId)
        {
            // TODO
        }

        [Route("Game/{gameId}/Guess")]
        [HttpPost]
        public ActionResult Guess(int gameId, [FromForm] string playerName, [FromForm] Guess guess)
        {
            return Ok(_gameService.TryEvaluateGuess(gameId, playerName, guess));
        }

        [Route("Game/{gameId}/Score")]
        [HttpGet]
        public void Score(int gameId)
        {
            // TODO
        }

    }
}
