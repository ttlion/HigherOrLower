using HigherOrLower.Entities.Cards;
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

        public void CreateGameCard(Guid gameId, int cardId, int drawOrder)
            => InsertAndSubmit(new GameCard(gameId, cardId, drawOrder));

        public int GetHighestGameDisplayId()
            => _gamesTable.Max(x => (int?) x.DisplayId) ?? 0;

        public List<int> GetAllGameCardsIds(Guid gameId)
            => _gameCardsTable.Where(x => x.GameId == gameId).Select(x => x.CardId).ToList();
    }
}
