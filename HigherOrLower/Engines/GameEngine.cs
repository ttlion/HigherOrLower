using HigherOrLower.Dtos;
using HigherOrLower.Entities.Games;
using HigherOrLower.Repositories.Cards;
using HigherOrLower.Repositories.Games;
using HigherOrLower.Utils.Enums;
using HigherOrLower.Utils.Extensions;
using HigherOrLower.Utils.Wrappers;

namespace HigherOrLower.Engines
{
    public class GameEngine : IGameEngine
    {
        // I chose to have the GameEngine methods returning directly the DTOs that are displayed in by the API for simplicity.
        // If wanted to be 100% purist on this issue, I would have set of Dtos returned by the engine, and would create converters to convert from those engine's DTOs to the DTOs displayed by the API.
        // However, I also want to have what remains of my Sunday to have some rest :)

        private readonly IGameRepository _gameRepository;
        private readonly ICardRepository _cardRepository;

        public GameEngine(IGameRepository gameRepository, ICardRepository cardRepository)
        {
            _gameRepository = gameRepository;
            _cardRepository = cardRepository;
        }

        public IResultWithStatus<GameInfoDto, CreateNewGameStatus> CreateNewGame()
        {
            // could put this logic to generate the next displayId directly on DB
            // with some identity() field, but wanted to put it in the engine
            // because now I am using this logic do generate the next displayId,
            // but in the future it may be change
            var highestGameDisplayId = _gameRepository.GetHighestGameDisplayId();

            var newGame = _gameRepository.CreateGame(highestGameDisplayId + 1);

            (bool isLastCard, int nextCardId) = DrawNextGameCard(newGame.Id);

            var nextCard = _cardRepository.GetCard(nextCardId);

            return SuccessResultCreateNewGame(new GameInfoDto(newGame.DisplayId, nextCard.Name, !isLastCard, isLastCard));
        }

        public IResultWithStatus<GameInfoWithGuessResultDto, EvaluateGuessStatus> TryDrawNextCardAndEvaluateGuess(int gameDisplayId, string playerName, Guess guess)
        {
            var game = _gameRepository.TryGetGame(gameDisplayId);

            if (game == null)
            {
                return ErrorResultEvaluateGuess(EvaluateGuessStatus.ErrorGameDoesNotExist);
            }

            if (game.IsFinished)
            {
                return ErrorResultEvaluateGuess(EvaluateGuessStatus.ErrorGameIsFinished);
            }

            var allPlayers = _gameRepository.GetAllGamePlayers(game.Id);

            var status = game.CanAddNewPlayers
                ? CloseTableOrAddNewPlayer(game.Id, playerName, allPlayers, out var player)
                : ValidateExistingPlayer(game.Id, playerName, allPlayers, out player);

            if (status != EvaluateGuessStatus.Success)
            {
                return ErrorResultEvaluateGuess(status);
            }

            var previousGameCardId = _gameRepository.GetLatestGameCardId(game.Id);

            (bool currentCardIsLastCard, int currentGameCardId) = DrawNextGameCard(game.Id);

            var cards = _cardRepository.GetCards([currentGameCardId, previousGameCardId]);

            var currentCard = cards[currentGameCardId];
            var previousCard = cards[previousGameCardId];

            var guessResult = DetermineGuessResult(guess, currentCard.Value, previousCard.Value);
            UpdatePlayerScore(guessResult, player.Id);

            UpdateCurrentPlayer(allPlayers);

            return SuccessResultEvaluateGuess(new GameInfoWithGuessResultDto(game.DisplayId, currentCard.Name, game.CanAddNewPlayers, currentCardIsLastCard, guessResult));
        }

        public IResultWithStatus<GameInfoWithPlayersInfoDto, GetGameInfoStatus> TryGetGameInfo(int gameDisplayId)
        {
            var game = _gameRepository.TryGetGame(gameDisplayId);

            if (game == null)
            {
                return ErrorResultTryGetGameInfo(GetGameInfoStatus.ErrorGameDoesNotExist);
            }

            var players = _gameRepository
                .GetAllGamePlayers(game.Id)
                .OrderBy(x => x.OrderInGame)
                .Select(x => new PlayerInfoDto(x.Name, x.Score, x.OrderInGame, !game.IsFinished && x.IsCurrentMove))
                .ToList();

            var currentCardId = _gameRepository.GetLatestGameCardId(game.Id);
            var currentCard = _cardRepository.GetCard(currentCardId);

            return SuccessResultTryGetGameInfo(new GameInfoWithPlayersInfoDto(game.DisplayId, currentCard.Name, game.CanAddNewPlayers, game.IsFinished, players));
        }

        private (bool IsLastCard, int CardId) DrawNextGameCard(Guid gameId)
        {
            // Could have made this some static readonly int, but wanted to keep this generic (for example, if, in the future,
            // we want more than 1 deck, just need to add the cards to the db, this code remains the same)
            var totalNumberOfCards = _cardRepository.GetTotalNumberOfCards();

            var cardsInGame = _gameRepository.GetAllGameCardsIds(gameId);

            var nextCardId = Enumerable.Range(0, totalNumberOfCards).Except(cardsInGame).ToList().GetRandomElement();

            var updatedNumberOfCardsInGame = cardsInGame.Count + 1;

            var isLastCard = updatedNumberOfCardsInGame == totalNumberOfCards;
            
            _gameRepository.CreateGameCard(gameId, nextCardId, updatedNumberOfCardsInGame);

            if (isLastCard)
            {
                _gameRepository.MarkGameFinished(gameId);
            }

            return (isLastCard, nextCardId);
        }

        private EvaluateGuessStatus CloseTableOrAddNewPlayer(Guid gameId, string playerName, IList<IPlayer> allPlayers, out IPlayer? player)
        {
            player = allPlayers.FirstOrDefault(x => x.Name == playerName);

            if (player != null)
            {
                if (player.OrderInGame != allPlayers.Min(x => x.OrderInGame))
                {
                    return EvaluateGuessStatus.ErrorNotProperPlayerToCloseTable;
                }

                _gameRepository.MarkGameCannotAddNewPlayers(gameId);
                player.IsCurrentMove = true;
                return EvaluateGuessStatus.Success;
            }

            player = _gameRepository.AddPlayerToGame(gameId, playerName, (allPlayers.Max(x => (int?)x.OrderInGame) ?? 0) + 1);
            allPlayers.Add(player);
            return EvaluateGuessStatus.Success;
        }

        private EvaluateGuessStatus ValidateExistingPlayer(Guid gameId, string playerName, IList<IPlayer> allPlayers, out IPlayer? player)
        {
            player = allPlayers.FirstOrDefault(x => x.Name == playerName);

            if (player == null)
            {
                return EvaluateGuessStatus.ErrorCannotAddNewPlayers;
            }

            var playerId = player.Id;
            if (allPlayers.Any(x => x.IsCurrentMove && x.Id != playerId))
            {
                return EvaluateGuessStatus.ErrorAnotherPlayersTurn;
            }

            return EvaluateGuessStatus.Success;
        }

        private static GuessResult DetermineGuessResult(Guess guess, int currentCardValue, int previousCardValue)
        {
            return (guess == Guess.Higher && currentCardValue >= previousCardValue)
                || (guess == Guess.Lower && currentCardValue <= previousCardValue)
                    ? GuessResult.Correct
                    : GuessResult.Incorrect;
        }

        private void UpdatePlayerScore(GuessResult guessResult, Guid playerId)
        {
            if (guessResult == GuessResult.Correct)
            {
                _gameRepository.IncrementPlayerScore(playerId);
            }
        }

        private void UpdateCurrentPlayer(IList<IPlayer> players)
        {
            var orderedPlayers = players.OrderBy(x => x.OrderInGame).ToList();

            var positionOfCurrentPlayer = orderedPlayers.FindIndex(x => x.IsCurrentMove);

            if (positionOfCurrentPlayer < 0)
            {
                return;
            }

            var positionOfNextPlayer = positionOfCurrentPlayer + 1 < orderedPlayers.Count ? positionOfCurrentPlayer + 1 : 0;

            _gameRepository.SetPlayerIsCurrentMoveValue(orderedPlayers[positionOfCurrentPlayer].Id, false);
            _gameRepository.SetPlayerIsCurrentMoveValue(orderedPlayers[positionOfNextPlayer].Id, true);
        }

        private static ResultWithStatus<GameInfoDto, CreateNewGameStatus> SuccessResultCreateNewGame(GameInfoDto result)
        {
            return ResultWithStatusSuccess(result, CreateNewGameStatus.Success);
        }

        private static ResultWithStatus<GameInfoWithGuessResultDto, EvaluateGuessStatus> SuccessResultEvaluateGuess(GameInfoWithGuessResultDto result)
        {
            return ResultWithStatusSuccess(result, EvaluateGuessStatus.Success);
        }

        private static ResultWithStatus<GameInfoWithPlayersInfoDto, GetGameInfoStatus> SuccessResultTryGetGameInfo(GameInfoWithPlayersInfoDto result)
        {
            return ResultWithStatusSuccess(result, GetGameInfoStatus.Success);
        }

        private static ResultWithStatus<T, U> ResultWithStatusSuccess<T, U>(T result, U status) where T : new()
        {
            return ResultWithStatus<T, U>.Success(result, status);
        }

        private static ResultWithStatus<GameInfoWithGuessResultDto, EvaluateGuessStatus> ErrorResultEvaluateGuess(EvaluateGuessStatus errorStatus)
        {
            return ResultWithStatusError<GameInfoWithGuessResultDto, EvaluateGuessStatus>(errorStatus);
        }

        private static ResultWithStatus<GameInfoWithPlayersInfoDto, GetGameInfoStatus> ErrorResultTryGetGameInfo(GetGameInfoStatus errorStatus)
        {
            return ResultWithStatusError<GameInfoWithPlayersInfoDto, GetGameInfoStatus>(errorStatus);
        }

        private static ResultWithStatus<T, U> ResultWithStatusError<T, U>(U errorStatus) where T : new()
        {
            return ResultWithStatus<T, U>.Error(errorStatus);
        }
    }
}
