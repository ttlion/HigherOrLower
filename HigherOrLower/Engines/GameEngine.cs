using HigherOrLower.Dtos;
using HigherOrLower.Repositories.Cards;
using HigherOrLower.Repositories.Games;
using HigherOrLower.Utils.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace HigherOrLower.Engines
{
    public class GameEngine : IGameEngine
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICardRepository _cardRepository;

        public GameEngine(IGameRepository gameRepository, ICardRepository cardRepository)
        {
            _gameRepository = gameRepository;
            _cardRepository = cardRepository;
        }

        public GameWithNextCardDto? TryCreateNewGame()
        {
            // could put this logic to generate the next displayId directly on DB
            // with some identity() field, but wanted to put it in the engine
            // because now I am using this logic do generate the next displayId,
            // but in the future it may be change
            var highestGameDisplayId = _gameRepository.GetHighestGameDisplayId();

            var newGame = _gameRepository.CreateGame(highestGameDisplayId + 1);

            var nextCardId = TryDrawNextGameCard(newGame.Id);

            if (nextCardId is null)
            {
                return null;
            }

            var nextCardName = _cardRepository.GetCard(nextCardId.Value).Name;
            return new GameWithNextCardDto(newGame.DisplayId, nextCardName);
        }

        public int? TryDrawNextGameCard(Guid gameId)
        {
            // could have made this some static readonly int,
            // but wanted to keep this generic (for example, if, in the future,
            // we want more than 1 deck, just need to add the cards to the db,
            // this code remains the same)
            var totalNumberOfCards = _cardRepository.GetTotalNumberOfCards();

            var cardsInGame = _gameRepository.GetAllGameCardsIds(gameId);

            var cardsToDraw = Enumerable.Range(0, totalNumberOfCards).Except(cardsInGame).ToList();

            if (cardsToDraw.IsNullOrEmpty())
            {
                return null;
            }

            var nextCardId = cardsToDraw.GetRandomElement();
            _gameRepository.CreateGameCard(gameId, nextCardId, cardsInGame.Count + 1);
            return nextCardId;
        }
    }
}
