namespace HigherOrLower.Dtos
{
    public class GameInfoDto
    {
        public int GameId { get; set; }

        public string CurrentCard { get; set; }

        public bool CanAddNewPlayers { get; set; }

        public bool IsGameFinished { get; set; }

        public GameInfoDto() : this (-1, string.Empty, false, false)
        {

        }

        public GameInfoDto(int gameId, string currentCard, bool canAddNewPlayers, bool isGameFinished)
        {
            GameId = gameId;
            CurrentCard = currentCard;
            CanAddNewPlayers = canAddNewPlayers;
            IsGameFinished = isGameFinished;
        }
    }
}
