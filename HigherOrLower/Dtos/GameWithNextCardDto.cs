namespace HigherOrLower.Dtos
{
    public class GameWithNextCardDto
    {
        public int Id { get; }

        public string NextCard { get; }

        public bool IsLastCard { get; }

        public GameWithNextCardDto() : this (-1, string.Empty, false) { }

        public GameWithNextCardDto(int id, string nextCard, bool isLastCard)
        {
            Id = id;
            NextCard = nextCard;
            IsLastCard = isLastCard;
        }
    }
}
