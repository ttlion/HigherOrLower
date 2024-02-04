using HigherOrLower.Entities.Cards;
using HigherOrLower.Entities.Games;

namespace HigherOrLower.Infrastructure.Database
{
    public interface IHigherOrLowerDbContext
    {
        IQueryable<Card> Cards { get; }
        
        IQueryable<GameCard> GameCards { get; }
        
        IQueryable<Game> Games { get; }
        
        IQueryable<Player> Players { get; }

        void InsertAndSubmit<T>(T data);

        void SubmitChanges();
    }
}