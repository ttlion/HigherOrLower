namespace HigherOrLower.Entities.Games
{
    public class GameCard : IGameCard
    {
        public Guid GameId { get; set; }

        public int CardId { get; set; }

        public int DrawOrder { get; set; }

        public GameCard(Guid gameId, int cardId, int drawOrder)
        {
            GameId = gameId;
            CardId = cardId;
            DrawOrder = drawOrder;
        }
    }
}
