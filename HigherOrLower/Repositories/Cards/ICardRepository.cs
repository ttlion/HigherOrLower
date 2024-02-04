using HigherOrLower.Entities.Cards;

namespace HigherOrLower.Repositories.Cards
{
    public interface ICardRepository
    {
        int GetTotalNumberOfCards();

        public ICard GetCard(int cardId);

        IDictionary<int, ICard> GetCards(List<int> cardIds);
    }
}