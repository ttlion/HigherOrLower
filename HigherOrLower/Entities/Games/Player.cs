namespace HigherOrLower.Entities.Games
{
    public class Player : IPlayer
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }

        public int Name { get; set; }

        public int Score { get; set; }

        public int OrderInGame { get; set; }

        public int IsCurrentMove { get; set; }
    }
}
