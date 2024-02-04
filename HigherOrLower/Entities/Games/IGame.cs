
namespace HigherOrLower.Entities.Games
{
    public interface IGame
    {
        Guid Id { get; set; }
        
        int DisplayId { get; set; }

        bool CanAddNewPlayers { get; set; }

        bool IsFinished { get; set; }
    }
}