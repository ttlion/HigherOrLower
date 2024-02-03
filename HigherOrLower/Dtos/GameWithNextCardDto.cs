namespace HigherOrLower.Dtos
{
    public class GameWithNextCardDto
    {
        public int Id { get; set; }
        public string NextCard { get; set; }

        public GameWithNextCardDto(int id, string nextCard)
        {
            Id = id;
            NextCard = nextCard;
        }
    }
}
