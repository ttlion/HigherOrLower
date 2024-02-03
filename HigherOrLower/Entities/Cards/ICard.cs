namespace HigherOrLower.Entities.Cards
{
    public interface ICard
    {
        int Id { get; set; }
        string Name { get; set; }
        int Value { get; set; }
    }
}