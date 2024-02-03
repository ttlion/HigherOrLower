using HigherOrLower.Entities.Cards;

namespace HigherOrLower.Repositories.Cards
{
    public interface ICardRepository
    {
        ICard GetCard(int cardId);

        int GetTotalNumberOfCards();
    }
}