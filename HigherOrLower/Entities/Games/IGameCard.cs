
namespace HigherOrLower.Entities.Games
{
    public interface IGameCard
    {
        Guid GameId { get; set; }

        int CardId { get; set; }
        
        int DrawOrder { get; set; }
    }
}