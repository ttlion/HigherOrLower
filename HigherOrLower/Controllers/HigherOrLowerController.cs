using HigherOrLower.Services;
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
            var result = _gameService.TryCreateNewGame();
            return result != default ? Ok(result) : Problem();
        }

        [Route("Game/{gameId}/NextCard")]
        [HttpGet]
        public void NextCard(int gameId)
        {
            // TODO
        }

        [Route("Game/{gameId}/Guess")]
        [HttpPost]
        public void Guess(int gameId)
        {
            // TODO
        }

        [Route("Game/{gameId}/Score")]
        [HttpGet]
        public void Score(int gameId)
        {
            // TODO
        }

    }
}
