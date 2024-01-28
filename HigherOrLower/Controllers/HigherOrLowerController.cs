using Microsoft.AspNetCore.Mvc;

namespace HigherOrLower.Controllers
{
    [ApiController]
    public class HigherOrLowerController : ControllerBase
    {
        private readonly ILogger<HigherOrLowerController> _logger;

        public HigherOrLowerController(ILogger<HigherOrLowerController> logger)
        {
            _logger = logger;
        }

        [Route("NewGame")]
        [HttpPost]
        public void NewGame()
        {
            // TODO
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
