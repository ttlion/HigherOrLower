namespace HigherOrLower.Dtos
{
    public class GameInfoWithPlayersInfoDto : GameInfoDto
    {
        public List<PlayerInfoDto> Players { get; } = new List<PlayerInfoDto>();

        public GameInfoWithPlayersInfoDto() : base() 
        {

        }
        
        public GameInfoWithPlayersInfoDto(int id, string currentCard, bool isGameFinished, List<PlayerInfoDto> playersInfo) : base(id, currentCard, isGameFinished)
        {
            Players = playersInfo;
        }
    }
}
