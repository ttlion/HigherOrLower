namespace HigherOrLower.Dtos
{
    public class GameInfoDto
    {
        public int GameId { get; }

        public string CurrentCard { get; }

        public bool IsGameFinished { get; }

        public GameInfoDto() : this (-1, string.Empty, false)
        {

        }

        public GameInfoDto(int gameId, string currentCard, bool isGameFinished)
        {
            GameId = gameId;
            CurrentCard = currentCard;
            IsGameFinished = isGameFinished;
        }
    }
}
