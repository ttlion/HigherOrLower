using FluentAssertions;
using HigherOrLower.Controllers;
using HigherOrLower.Dtos;
using HigherOrLower.Engines;
using HigherOrLower.Entities.Cards;
using HigherOrLower.Entities.Games;
using HigherOrLower.Infrastructure.Database;
using HigherOrLower.Repositories.Cards;
using HigherOrLower.Repositories.Games;
using HigherOrLower.Services;
using HigherOrLower.Utils.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace HigherOrLower.Tests.Controllers
{
    public class HigherOrLowerControllerTests
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    { "DatabaseType", "InMemory" },
                    { "InMemoryDatabaseName", $"Db{nameof(HigherOrLowerControllerTests)}" }
                })
            .Build();

        private static readonly List<Card> _deck = new List<Card>
        {
            new Card() { Id = 0, Name = "10 of diamonds", Value = 10 },
            new Card() { Id = 1, Name = "queen of hearts", Value = 12 },
            new Card() { Id = 2, Name = "ace of spades", Value = 13 },
            new Card() { Id = 3, Name = "2 of spades", Value = 2 },
            new Card() { Id = 4, Name = "3 of hearts", Value = 3 },
            new Card() { Id = 5, Name = "6 of clubs", Value = 3 },
        };

        private static readonly Guid _gameIdOfGameAlreadyFinished = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfGameAlreadyFinished = 1;

        private static readonly Guid _gameIdOfGameWhereCanAddNewPlayers = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfGameWhereCanAddNewPlayers = 2;

        private static readonly Guid _gameIdOfAnotherGameWhereCanAddNewPlayers = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfAnotherGameWhereCanAddNewPlayers = 3;

        private static readonly Guid _gameIdOfGameWithSeveralPlayersAndScoresAndIsSomeonesTurn = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn = 4;

        private static readonly Guid _gameIdOfAnotherGameWithSeveralPlayersAndScoresAndIsSomeonesTurn = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn = 5;

        private static readonly Guid _gameIdOfYetAnotherGameWithSeveralPlayersAndScoresAndIsSomeonesTurn = Guid.NewGuid();
        private static readonly int _gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn = 6;

        private static readonly int _maxDisplayIdOnOriginalDb = _gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn;

        static HigherOrLowerControllerTests()
        {
            var higherOrLowerDbContext = new HigherOrLowerDbContext(_configuration);

            _deck.ForEach(card => higherOrLowerDbContext.Cards.Add(card));
            higherOrLowerDbContext.SaveChanges();

            higherOrLowerDbContext.Games.Add(new Game(_gameDisplayIdOfGameAlreadyFinished) { Id = _gameIdOfGameAlreadyFinished, CanAddNewPlayers = false, IsFinished = true });
            higherOrLowerDbContext.Players.Add(new Player(_gameIdOfGameAlreadyFinished, "Dummy1", 1));
            higherOrLowerDbContext.Players.Add(new Player(_gameIdOfGameAlreadyFinished, "Dummy2", 2));
            higherOrLowerDbContext.GameCards.Add(new GameCard(_gameIdOfGameAlreadyFinished, 1, 1));

            // Setting up 2 equal games because one of them will be used for getting errors, the other to add some new player, and the tests should be unit and the order in which they run should be indiferent
            SetupGamesWhereCanAddNewPlayers(_gameDisplayIdOfGameWhereCanAddNewPlayers, _gameIdOfGameWhereCanAddNewPlayers);
            SetupGamesWhereCanAddNewPlayers(_gameDisplayIdOfAnotherGameWhereCanAddNewPlayers, _gameIdOfAnotherGameWhereCanAddNewPlayers);

            void SetupGamesWhereCanAddNewPlayers(int displayId, Guid gameId)
            {
                higherOrLowerDbContext.Games.Add(new Game(displayId) { Id = gameId, CanAddNewPlayers = true, IsFinished = false });
                higherOrLowerDbContext.Players.Add(new Player(gameId, "John", 1));
                higherOrLowerDbContext.Players.Add(new Player(gameId, "Mary", 2));
                higherOrLowerDbContext.Players.Add(new Player(gameId, "Susan", 3));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 1, 1));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 3, 2));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 5, 3));
            }


            // Setting up 3 equal games because one of them will be used for getting errors, the others to add some new player, and the tests should be unit and the order in which they run should be indiferent
            SetupGamesWithSeveralPlayersAndScoresAndIsSomeonesTurn(_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, _gameIdOfGameWithSeveralPlayersAndScoresAndIsSomeonesTurn);
            SetupGamesWithSeveralPlayersAndScoresAndIsSomeonesTurn(_gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, _gameIdOfAnotherGameWithSeveralPlayersAndScoresAndIsSomeonesTurn);
            SetupGamesWithSeveralPlayersAndScoresAndIsSomeonesTurn(_gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, _gameIdOfYetAnotherGameWithSeveralPlayersAndScoresAndIsSomeonesTurn);

            void SetupGamesWithSeveralPlayersAndScoresAndIsSomeonesTurn(int displayId, Guid gameId)
            {
                higherOrLowerDbContext.Games.Add(new Game(displayId) { Id = gameId, CanAddNewPlayers = false, IsFinished = false });
                higherOrLowerDbContext.Players.Add(new Player(gameId, "Mario", 1) { Score = 3 });
                higherOrLowerDbContext.Players.Add(new Player(gameId, "Joan", 2) { IsCurrentMove = true, Score = 2 });
                higherOrLowerDbContext.Players.Add(new Player(gameId, "Spencer", 3) { Score = 1 });
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 1, 1));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 3, 2));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 5, 3));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 0, 4));
                higherOrLowerDbContext.GameCards.Add(new GameCard(gameId, 2, 5));
            }

            higherOrLowerDbContext.SaveChanges();
        }

        private readonly HigherOrLowerController _controller;

        public HigherOrLowerControllerTests()
        {
            _controller = new HigherOrLowerController(
                new GameService(
                    new GameEngine(
                        new GameRepository(new HigherOrLowerDbContext(_configuration)),
                        new CardRepository(new HigherOrLowerDbContext(_configuration))
                    )
                )
            );
        }

        [Fact]
        public void CreatingGameReturnsNewGameInfoWithAnExistingCardDrawn()
        {
            var gameInfo = ParseJsonResult<GameInfoDto>(_controller.NewGame());

            gameInfo.GameId.Should().Be(_maxDisplayIdOnOriginalDb + 1);
            _deck.Select(x => x.Name).Should().Contain(gameInfo.CurrentCard);
            gameInfo.CanAddNewPlayers.Should().BeTrue();
            gameInfo.IsGameFinished.Should().BeFalse();
        }

        [Fact]
        public void ErrorWhenTryingToMakeGuessForNonExistingGame()
        {
            var nonExistingGameDisplayId = 876;
            var error = ParseJsonResult<TestClassWithErrorMessage>(_controller.Guess(nonExistingGameDisplayId, "Dummy1", Guess.Higher));
            error.Message.Should().Be($"Cannot make guess because game {nonExistingGameDisplayId} does not exist");
        }

        [Fact]
        public void ErrorWhenTryingToMakeGuessForFinishedGame()
        {
            var error = ParseJsonResult<TestClassWithErrorMessage>(_controller.Guess(_gameDisplayIdOfGameAlreadyFinished, "Dummy1", Guess.Higher));
            error.Message.Should().Be($"Cannot make guess because game {_gameDisplayIdOfGameAlreadyFinished} is already finished (check GameInfo endpoint)");
        }

        [Fact]
        public void ErrorWhenTryingToMakeGuessByClosingTableWithWrongPlayer()
        {
            var playerName = "Mary";
            var error = ParseJsonResult<TestClassWithErrorMessage>(_controller.Guess(_gameDisplayIdOfGameWhereCanAddNewPlayers, playerName, Guess.Higher));
            error.Message.Should().Be($"Cannot make guess because table {playerName} is already playing, but it is not the one that should close the table cycle to start the 2nd round of guesses (check GameInfo endpoint)");
        }

        [Fact]
        public void ClosingTableByMakingGuessWithProperPlayer()
        {
            var playerName = "John";
            var gameInfoWithGuessResultDto = ParseJsonResult<GameInfoWithGuessResultDto>(_controller.Guess(_gameDisplayIdOfAnotherGameWhereCanAddNewPlayers, playerName, Guess.Higher));

            gameInfoWithGuessResultDto.GuessResult.Should().BeOneOf(GuessResult.Correct, GuessResult.Incorrect); // There is a unit test to check the proper value (for the case there is only 1 card left, that is deterministic, here it is not)
            gameInfoWithGuessResultDto.GameId.Should().Be(_gameDisplayIdOfAnotherGameWhereCanAddNewPlayers);
            _deck.Select(x => x.Name).Should().Contain(gameInfoWithGuessResultDto.CurrentCard);
            gameInfoWithGuessResultDto.CanAddNewPlayers.Should().BeFalse();
            gameInfoWithGuessResultDto.IsGameFinished.Should().BeFalse();
        }

        [Fact]
        public void ErrorWhenTryingToMakeGuessBecauseItIsAnotherPlayersTurn()
        {
            var playerName = "Mario";
            var error = ParseJsonResult<TestClassWithErrorMessage>(_controller.Guess(_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, playerName, Guess.Higher));
            error.Message.Should().Be($"Cannot make guess because it is not {playerName}'s turn in game {_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn} (check GameInfo endpoint)");

            playerName = "Spencer";
            error = ParseJsonResult<TestClassWithErrorMessage>(_controller.Guess(_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, playerName, Guess.Higher));
            error.Message.Should().Be($"Cannot make guess because it is not {playerName}'s turn in game {_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn} (check GameInfo endpoint)");
        }

        [Fact]
        public void MakingIncorrectGuessAndEndingGame()
        {
            // For this game, according to the data inserted, current card is "ace of spades" (value=13), and the next card is "3 of hearts" (the only one missing), so the correct guess is lower

            var expectedGuessResult = GuessResult.Incorrect;
   
            var playerName = "Joan";
            
            var gameInfoWithGuessResultDto = ParseJsonResult<GameInfoWithGuessResultDto>(_controller.Guess(_gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, playerName, Guess.Higher));
            ValidateGameInfoWithGuessResultDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(gameInfoWithGuessResultDto, expectedGuessResult, _gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn);
            
            var gameInfoWithPlayersInfoDto = ParseJsonResult<GameInfoWithPlayersInfoDto>(_controller.GameInfo(_gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn));
            ValidateGameInfoWithPlayersInfoDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(gameInfoWithPlayersInfoDto, _gameDisplayIdOfAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, guessResult: expectedGuessResult);

        }

        [Fact]
        public void MakingCorrectGuessAndEndingGame()
        {
            // For this game, according to the data inserted, current card is "ace of spades" (value=13), and the next card is "3 of hearts" (the only one missing), so the correct guess is lower

            var expectedGuessResult = GuessResult.Correct;
            
            var playerName = "Joan";

            var gameInfoWithGuessResultDto = ParseJsonResult<GameInfoWithGuessResultDto>(_controller.Guess(_gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, playerName, Guess.Lower));
            ValidateGameInfoWithGuessResultDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(gameInfoWithGuessResultDto, expectedGuessResult, _gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn);

            var gameInfoWithPlayersInfoDto = ParseJsonResult<GameInfoWithPlayersInfoDto>(_controller.GameInfo(_gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn));
            ValidateGameInfoWithPlayersInfoDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(gameInfoWithPlayersInfoDto, _gameDisplayIdOfYetAnotherGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, guessResult: expectedGuessResult);
        }

        [Fact]
        public void ErrorTryingToGetGameInfoOfNonExistingGame()
        {
            var gameId = 999;
            var error = ParseJsonResult<TestClassWithErrorMessage>(_controller.GameInfo(gameId));
            error.Message.Should().Be($"Cannot get game info because game {gameId} does not exist");
        }

        [Fact]
        public void SucessGettingGameInfo()
        {
            var gameInfoWithPlayersInfoDto = ParseJsonResult<GameInfoWithPlayersInfoDto>(_controller.GameInfo(_gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn));
            ValidateGameInfoWithPlayersInfoDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(gameInfoWithPlayersInfoDto, _gameDisplayIdOfGameWithSeveralPlayersAndScorresAndIsSomeonesTurn, isAfterPlaying: false);
        }

        private void ValidateGameInfoWithPlayersInfoDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(GameInfoWithPlayersInfoDto gameInfoWithPlayersInfoDto, int gameId, bool isAfterPlaying = true, GuessResult guessResult = GuessResult.Correct)
        {
            gameInfoWithPlayersInfoDto.GameId.Should().Be(gameId);
            _deck.Select(x => x.Name).Should().Contain(gameInfoWithPlayersInfoDto.CurrentCard);
            gameInfoWithPlayersInfoDto.CanAddNewPlayers.Should().BeFalse();
            gameInfoWithPlayersInfoDto.IsGameFinished.Should().Be(isAfterPlaying);

            gameInfoWithPlayersInfoDto.Players.Should().HaveCount(3);

            var firstPlayer = gameInfoWithPlayersInfoDto.Players.Single(x => x.OrderInGame == 1);
            firstPlayer.Name.Should().Be("Mario");
            firstPlayer.Score.Should().Be(3);
            firstPlayer.IsCurrentPlayerToMove.Should().BeFalse();

            var secondPlayer = gameInfoWithPlayersInfoDto.Players.Single(x => x.OrderInGame == 2);
            secondPlayer.Name.Should().Be("Joan");
            secondPlayer.Score.Should().Be(isAfterPlaying && guessResult == GuessResult.Correct ? 3 : 2);
            secondPlayer.IsCurrentPlayerToMove.Should().Be(!isAfterPlaying);

            var thirdPlayer = gameInfoWithPlayersInfoDto.Players.Single(x => x.OrderInGame == 3);
            thirdPlayer.Name.Should().Be("Spencer");
            thirdPlayer.Score.Should().Be(1);
            thirdPlayer.IsCurrentPlayerToMove.Should().BeFalse();
        }

        private void ValidateGameInfoWithGuessResultDtoForGameWithSeveralPlayersAndScorresAndIsSomeonesTurn(GameInfoWithGuessResultDto gameInfoWithGuessResultDto, GuessResult guessResult, int gameId)
        {
            gameInfoWithGuessResultDto.GuessResult.Should().BeOneOf(guessResult);
            gameInfoWithGuessResultDto.GameId.Should().Be(gameId);
            _deck.Select(x => x.Name).Should().Contain(gameInfoWithGuessResultDto.CurrentCard);
            gameInfoWithGuessResultDto.CanAddNewPlayers.Should().BeFalse();
            gameInfoWithGuessResultDto.IsGameFinished.Should().BeTrue();
        }

        private static T ParseJsonResult<T>(ActionResult result)
        {
            return JsonConvert.DeserializeObject<T>(((OkObjectResult) result).Value.ToString());
        } 

        private class TestClassWithErrorMessage
        {
            public string Message { get; set; }
        }

    }
}
