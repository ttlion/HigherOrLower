using HigherOrLower.Dtos;
using HigherOrLower.Services;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

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

        /// <summary>
        /// Creates a new game
        /// </summary>
        /// <remarks>
        ///
        /// On success, API returns:
        /// - Result: The new game created. It will contain the id that should be used to access the game, and the first card drawn.
        /// - IsError: false
        /// - Status: String indicating success
        /// 
        /// On insucess, API returns:
        /// - IsError: true
        /// - Status: some informative error message explaining the reason
        /// - Result: some default data (unusable)
        /// 
        /// </remarks>
        [Route("[Action]")]
        [HttpPost]
        public IResultWithStatus<GameInfoDto, string> NewGame()
        {
            return _gameService.CreateNewGame();
        }

        /// <summary>
        /// Tries to make a guess for the specified player
        /// </summary>
        /// <remarks>
        /// 
        /// blablabla 
        /// 
        /// explain thing of table being filled or not
        /// 
        /// explain thing of being current player turn or not
        /// 
        /// explain that cannot make move if game finished
        ///
        /// On success, API returns:
        /// - Result: The result of the guess and the game updated, which includes the new card drawn.
        /// - IsError: false
        /// - Status: String indicating success
        /// 
        /// On insucess, API returns:
        /// - IsError: true
        /// - Status: some informative error message explaining the reason
        /// - Result: some default data (unusable)
        /// 
        /// </remarks>
        [Route("Game/{gameId}/[Action]")]
        [HttpPost]
        public IResultWithStatus<GameInfoWithGuessResultDto, string> Guess(int gameId, [FromForm] string playerName, [FromForm] Guess guess)
        {
            return _gameService.TryEvaluateGuess(gameId, playerName, guess);
        }

        /// <summary>
        /// Gets the relevant game info, including a list of the players and their scores
        /// </summary>
        /// <remarks>
        /// 
        /// blablabla 
        /// 
        /// explain all fields?
        ///
        /// On success, API returns:
        /// - Result: All info about the game, including a list of the players and their scores
        /// - IsError: false
        /// - Status: String indicating success
        /// 
        /// On insucess, API returns:
        /// - IsError: true
        /// - Status: some informative error message explaining the reason
        /// - Result: some default data (unusable)
        /// 
        /// </remarks>
        [Route("Game/{gameId}/[Action]")]
        [HttpGet]
        public IResultWithStatus<GameInfoWithPlayersInfoDto, string> GameInfo(int gameId)
        {
            return _gameService.GetGameInfo(gameId);
        }
    }
}
