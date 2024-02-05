using HigherOrLower.Dtos;
using HigherOrLower.Services;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Wrappers;
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

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <remarks>
        ///
        /// On success, API returns:
        /// - IsError: false
        /// - Status: String indicating success
        /// - Result: The new game created. It will contain the id that should be used to access the game, and the first card drawn.
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
        /// Try to make a guess for the specified player
        /// </summary>
        /// <remarks>
        /// 
        /// The game starts with canAddNewPlayers = true (see GameInfo endpoint).
        /// 
        /// The way of automatically adding a player to the game is by making a guess with a player name which is not in the player list of that game (while the canAddNewPlayers is true for that game).
        /// 
        /// While canAddNewPlayers is true, all players have their isCurrentPlayerToMove set to false, and there are two valid guesses:
        /// - A guess for a player whose name is not already in the player's list: the guess is made and that player is added to the players list
        /// - A guess for the first player of the game (the one with orderInGame = 1): only that player can close the "player cycle", in order for the game to be fair and ensure all players make a guess in the same order
        /// 
        /// The game assumes that there is a "player cycle", for the game to be fair.
        /// 
        /// In practice, if 3 players entered the game in the order PlayerA -> PlayerB -> PlayerC, then they should always guess in that order: 
        /// 
        /// A->B-C->A->B->C->A->B->C...
        /// 
        /// The game allows any number of players to join the game until the first player guesses a second time. In that moment, the list of players is closed, the game gets canAddNewPlayers = false, and only the player with isCurrentPlayerToMove = true is the one who can play.
        /// 
        /// If the game is finished, no player is allowed to make any guess.
        ///
        /// On success, API returns:
        /// - IsError: false
        /// - Status: String indicating success
        /// - Result: The result of the guess and the game updated, which includes the new card drawn.
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
        /// Get relevant game info, including a list of the players and their scores
        /// </summary>
        /// <remarks>
        /// 
        /// On success, API returns:
        /// - IsError: false
        /// - Status: String indicating success
        /// - Result: All info about the game, including a list of the players and their scores
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
