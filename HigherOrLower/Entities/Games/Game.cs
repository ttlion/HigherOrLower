namespace HigherOrLower.Entities.Games
{
    public class Game : IGame
    {
        public Game(int displayId)
        {
            DisplayId = displayId;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public int DisplayId { get; set; }

        public bool CanAddNewPlayers { get; set; } = true;

        public bool IsFinished { get; set; } = false;
    }
}
