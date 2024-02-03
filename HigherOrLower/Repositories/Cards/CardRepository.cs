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

        public ICard GetCard(int cardId)
            => _cardsTable.First(x => x.Id == cardId);

        public int GetTotalNumberOfCards()
            => _cardsTable.Count();
    }
}
