namespace HigherOrLower.Entities.Games
{
    public class Player : IPlayer
    {
        public Player(Guid gameId, string name, int orderInGame)
        {
            GameId = gameId;
            Name = name;
            OrderInGame = orderInGame;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid GameId { get; set; }

        public string Name { get; set; }

        public int Score { get; set; } = 0;

        public int OrderInGame { get; set; }

        public bool IsCurrentMove { get; set; } = false;
    }
}
