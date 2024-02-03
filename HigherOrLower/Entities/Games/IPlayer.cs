
namespace HigherOrLower.Entities.Games
{
    public interface IPlayer
    {
        Guid GameId { get; set; }
        Guid Id { get; set; }
        int IsCurrentMove { get; set; }
        int Name { get; set; }
        int OrderInGame { get; set; }
        int Score { get; set; }
    }
}