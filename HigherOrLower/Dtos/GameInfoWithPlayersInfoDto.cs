namespace HigherOrLower.Dtos
{
    public class GameInfoWithPlayersInfoDto : GameInfoDto
    {
        public List<PlayerInfoDto> Players { get; set; } = new List<PlayerInfoDto>();

        public GameInfoWithPlayersInfoDto() : base() 
        {

        }
        
        public GameInfoWithPlayersInfoDto(int id, string currentCard, bool canAddNewPlayers, bool isGameFinished, List<PlayerInfoDto> playersInfo) : base(id, currentCard, canAddNewPlayers, isGameFinished)
        {
            Players = playersInfo;
        }
    }
}
