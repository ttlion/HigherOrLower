
namespace HigherOrLower.Entities.Games
{
    public interface IPlayer
    {
        Guid Id { get; set; }
        
        Guid GameId { get; set; }

        string Name { get; set; }

        int Score { get; set; }
        
        int OrderInGame { get; set; }

        bool IsCurrentMove { get; set; }
    }
}