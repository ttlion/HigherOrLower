using HigherOrLower.Entities.Cards;
using HigherOrLower.Entities.Games;

namespace HigherOrLower.Repositories.Games
{
    public interface IGameRepository
    {
        IGame CreateGame(int displayId);

        void CreateGameCard(Guid gameId, int cardId, int drawOrder);
        
        int GetHighestGameDisplayId();

        List<int> GetAllGameCardsIds(Guid gameId);
    }
}
