using HigherOrLower.Entities.Games;
using HigherOrLower.Infrastructure.Database;

namespace HigherOrLower.Repositories.Games
{
    public class GameRepository : RepositoryBase, IGameRepository
    {
        private readonly IQueryable<Game> _gamesTable;
        private readonly IQueryable<GameCard> _gameCardsTable;
        private readonly IQueryable<Player> _playersTable;

        public GameRepository(IHigherOrLowerDbContext dc) : base(dc)
        {
            _gamesTable = dc.Games;
            _gameCardsTable = dc.GameCards;
            _playersTable = dc.Players;
        }

        public IGame CreateGame(int displayId)
        {
            var newGame = new Game(displayId);
            InsertAndSubmit(newGame);
            return newGame;
        }

        public IGame? TryGetGame(int displayId)
        {
            return _gamesTable.FirstOrDefault(x => x.DisplayId == displayId);
        }

        public void MarkGameCannotAddNewPlayers(Guid gameId)
        {
            var game = _gamesTable.First(x => x.Id == gameId);
            game.CanAddNewPlayers = false;
            SubmitChanges();
        }

        public void MarkGameFinished(Guid gameId)
        {
            var game = _gamesTable.First(x => x.Id == gameId);
            game.IsFinished = true;
            SubmitChanges();
        }

        public int GetHighestGameDisplayId()
        {
            return _gamesTable.Max(x => (int?)x.DisplayId) ?? 0;
        }


        public void CreateGameCard(Guid gameId, int cardId, int drawOrder)
        {
            InsertAndSubmit(new GameCard(gameId, cardId, drawOrder));
        }

        public IList<int> GetAllGameCardsIds(Guid gameId)
        {
            return _gameCardsTable.Where(x => x.GameId == gameId).Select(x => x.CardId).ToList();
        }

        public int GetLatestGameCardId(Guid gameId)
        {
            return _gameCardsTable.Where(x => x.GameId == gameId).OrderByDescending(x => x.DrawOrder).Select(x => x.CardId).FirstOrDefault();
        }


        public IPlayer AddPlayerToGame(Guid gameId, string playerName, int orderInGame)
        {
            var player = new Player(gameId, playerName, orderInGame);
            InsertAndSubmit(player);
            return player;
        }

        public IList<IPlayer> GetAllGamePlayers(Guid gameId)
        {
            return _playersTable.Where(x => x.GameId == gameId).ToList<IPlayer>();
        }

        public void IncrementPlayerScore(Guid playerId)
        {
            var player = _playersTable.First(x => x.Id == playerId);
            player.Score++;
            SubmitChanges();
        }

        public void SetPlayerIsCurrentMoveValue(Guid playerId, bool isCurrentMove)
        {
            var player = _playersTable.First(x => x.Id == playerId);
            player.IsCurrentMove = isCurrentMove;
            SubmitChanges();
        }
    }
}
