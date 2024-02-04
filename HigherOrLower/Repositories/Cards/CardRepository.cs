using HigherOrLower.Entities.Cards;
using HigherOrLower.Infrastructure.Database;

namespace HigherOrLower.Repositories.Cards
{
    public class CardRepository : RepositoryBase, ICardRepository
    {
        // Created this in a separate repo class (instead of the game repo class) to show I can separate concerns (cards are a "standalone" entity, dont need a game to exist)
        // Obviously, this costs some performance (if I had the cardsTable available in the game repo class, there are some operations in the game engine that would take only 1 hit in the DB instead of 2, by doing some joins)

        private readonly IQueryable<Card> _cardsTable;

        public CardRepository(IHigherOrLowerDbContext dc) : base(dc)
        {
            _cardsTable = dc.Cards;
        }

        public int GetTotalNumberOfCards()
        {
            return _cardsTable.Count();
        }

        public ICard GetCard(int cardId)
        {
            return _cardsTable.First(x => x.Id == cardId);
        }

        public IDictionary<int, ICard> GetCards(List<int> cardIds)
        {
            return _cardsTable.Where(x => cardIds.Contains(x.Id)).ToDictionary(x => x.Id, x => x as ICard);
        }
    }
}
