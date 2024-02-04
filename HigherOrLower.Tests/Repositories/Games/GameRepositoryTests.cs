using FluentAssertions;
using HigherOrLower.Entities.Cards;
using HigherOrLower.Entities.Games;
using HigherOrLower.Infrastructure.Database;
using HigherOrLower.Repositories.Games;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace HigherOrLower.Tests.Repositories.Games
{
    public class GameRepositoryTests
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    { "DatabaseType", "InMemory" },
                    { "InMemoryDatabaseName", $"Db{nameof(GameRepositoryTests)}" }
                })
            .Build();

        private static readonly Guid _testGameId;
        private static readonly int _testGameDisplayId;

        static GameRepositoryTests()
        {
            var higherOrLowerDbContext = new HigherOrLowerDbContext(_configuration);

            higherOrLowerDbContext.Cards.Add(new Card() { Id = 1, Name = "10 of diamonds", Value = 10 });
            higherOrLowerDbContext.Cards.Add(new Card() { Id = 2, Name = "queen of hearts", Value = 12 });
            higherOrLowerDbContext.Cards.Add(new Card() { Id = 3, Name = "ace of spades", Value = 13 });
            higherOrLowerDbContext.SaveChanges();
        }

        private readonly IGameRepository _gameRepository;

        public GameRepositoryTests()
        {
            _gameRepository = new GameRepository(new HigherOrLowerDbContext(_configuration));
        }

        [Fact]
        public void CreateGameTests()
        {
            var displayId = 1;
            var game = _gameRepository.CreateGame(displayId);

            Guid.TryParse(game.Id.ToString(), out _).Should().BeTrue();
            game.DisplayId.Should().Be(displayId);
            game.DisplayId.Should().Be(displayId);
            game.CanAddNewPlayers.Should().BeTrue();
            game.IsFinished.Should().BeFalse();
        }

        [Fact]
        public void TryGetGameTests()
        {
            var displayId = 2;
            var expectedGame = _gameRepository.CreateGame(displayId);

            var game = _gameRepository.TryGetGame(displayId);

            game.Id.Should().Be(expectedGame.Id);
            game.DisplayId.Should().Be(expectedGame.DisplayId);
            game.CanAddNewPlayers.Should().Be(expectedGame.CanAddNewPlayers);
            game.IsFinished.Should().Be(expectedGame.IsFinished);
        }

        [Fact]
        public void MarkGameCannotAddNewPlayersTests()
        {
            var displayId = 3;
            var game = _gameRepository.CreateGame(displayId);

            game.IsFinished.Should().BeFalse();

            _gameRepository.MarkGameFinished(game.Id);
            game.IsFinished.Should().BeTrue();
        }

        [Fact]
        public void GetHighestGameDisplayIdTests()
        {
            var largeDisplayId = 999999;
            var game = _gameRepository.CreateGame(largeDisplayId);
            _gameRepository.GetHighestGameDisplayId().Should().Be(largeDisplayId);
        }

        [Fact]
        public void GameCardsTests()
        {
            var displayId = 4;
            var game = _gameRepository.CreateGame(displayId);

            _gameRepository.CreateGameCard(game.Id, 3, 1);
            _gameRepository.CreateGameCard(game.Id, 2, 2);
            _gameRepository.CreateGameCard(game.Id, 1, 3);

            _gameRepository.GetAllGameCardsIds(game.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
            _gameRepository.GetLatestGameCardId(game.Id).Should().Be(1);
        }

        [Fact]
        public void PlayersAddAndGetTests()
        {
            var displayId = 5;
            var game = _gameRepository.CreateGame(displayId);

            var expectedPlayers = new List<IPlayer>
            {
                new Player(game.Id, "John", 1),
                new Player(game.Id, "Sarah", 2),
                new Player(game.Id, "Michael", 3),
            };

            var createdPlayers = 
                expectedPlayers
                .Select(player => _gameRepository.AddPlayerToGame(game.Id, player.Name, player.OrderInGame))
                .ToList();

            createdPlayers.Should().HaveSameCount(expectedPlayers);

            foreach (var expectedPlayer in expectedPlayers)
            {
                createdPlayers.Should().ContainSingle(player =>
                    player.Id != expectedPlayer.Id
                    && player.GameId == expectedPlayer.GameId
                    && player.Name == expectedPlayer.Name
                    && player.Score == 0
                    && player.OrderInGame == expectedPlayer.OrderInGame
                    && !player.IsCurrentMove
                );
            }

            var fetchedPlayers = _gameRepository.GetAllGamePlayers(game.Id);

            fetchedPlayers.Should().HaveSameCount(expectedPlayers);

            foreach (var expectedPlayer in expectedPlayers)
            {
                fetchedPlayers.Should().ContainSingle(player => 
                    player.Id != expectedPlayer.Id
                    && player.GameId == expectedPlayer.GameId
                    && player.Name == expectedPlayer.Name
                    && player.Score == 0
                    && player.OrderInGame == expectedPlayer.OrderInGame
                    && !player.IsCurrentMove
                );
            }
        }

        [Fact]
        public void IncrementPlayerScoreTests()
        {
            var displayId = 6;
            var game = _gameRepository.CreateGame(displayId);

            var player = _gameRepository.AddPlayerToGame(game.Id, "John", 1);
            player.Score.Should().Be(0);

            _gameRepository.IncrementPlayerScore(player.Id);
            player.Score.Should().Be(1);
        }

        [Fact]
        public void SetPlayerIsCurrentMoveValueTests()
        {
            var displayId = 7;
            var game = _gameRepository.CreateGame(displayId);

            var player = _gameRepository.AddPlayerToGame(game.Id, "John", 1);
            player.IsCurrentMove.Should().BeFalse();

            _gameRepository.SetPlayerIsCurrentMoveValue(player.Id, true);
            player.IsCurrentMove.Should().BeTrue();


            _gameRepository.SetPlayerIsCurrentMoveValue(player.Id, false);
            player.IsCurrentMove.Should().BeFalse();
        }
    }
}
