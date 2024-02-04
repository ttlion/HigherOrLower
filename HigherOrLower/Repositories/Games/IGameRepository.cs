using HigherOrLower.Entities.Cards;
using HigherOrLower.Entities.Games;

namespace HigherOrLower.Repositories.Games
{
    public interface IGameRepository
    {
        IGame CreateGame(int displayId);

        IGame? TryGetGame(int displayId);
        
        void MarkGameCannotAddNewPlayers(Guid gameId);
        
        void MarkGameFinished(Guid gameId);
        
        int GetHighestGameDisplayId();


        void CreateGameCard(Guid gameId, int cardId, int drawOrder);

        IList<int> GetAllGameCardsIds(Guid gameId);

        int GetLatestGameCardId(Guid gameId);


        IPlayer AddPlayerToGame(Guid gameId, string playerName, int orderInGame);

        IList<IPlayer> GetAllGamePlayers(Guid gameId);

        void IncrementPlayerScore(Guid playerId);

        void SetPlayerIsCurrentMoveValue(Guid playerId, bool isCurrentMove);
    }
}
