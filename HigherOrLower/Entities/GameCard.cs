namespace HigherOrLower.Entities
{
    public class GameCard
    {
		public Guid GameId { get; set; }

		public Guid CardId { get; set; }

		public int DrawOrder { get; set; }
    }
}
