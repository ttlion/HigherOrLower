using HigherOrLower.Entities.Cards;
using HigherOrLower.Infrastructure.Database;

namespace HigherOrLower.Repositories.Cards
{
    public class CardRepository : RepositoryBase, ICardRepository
    {
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
