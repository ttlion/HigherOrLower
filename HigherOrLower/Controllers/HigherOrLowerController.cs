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

        [Route("[Action]")]
        [HttpPost]
        public ActionResult NewGame()
        {
            return Ok(_gameService.CreateNewGame());
        }

        [Route("Game/{gameId}/[Action]")]
        [HttpPost]
        public ActionResult Guess(int gameId, [FromForm] string playerName, [FromForm] Guess guess)
        {
            return Ok(_gameService.TryEvaluateGuess(gameId, playerName, guess));
        }

        [Route("Game/{gameId}/[Action]")]
        [HttpGet]
        public ActionResult GameInfo(int gameId)
        {
            return Ok(_gameService.GetGameInfo(gameId));
        }
    }
}
