namespace HigherOrLower.Entities.Cards
{
    public class Card : ICard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}
