
namespace HigherOrLower.Entities.Games
{
    public interface IGame
    {
        int DisplayId { get; set; }
        Guid Id { get; set; }
        bool IsFinished { get; set; }
    }
}