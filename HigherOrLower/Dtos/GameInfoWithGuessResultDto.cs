using HigherOrLower.Utils.Enums;

namespace HigherOrLower.Dtos
{
    public class GameInfoWithGuessResultDto : GameInfoDto
    {
        public GuessResult GuessResult { get; set; }

        public GameInfoWithGuessResultDto() : base() 
        {
            
        }

        public GameInfoWithGuessResultDto(int id, string currentCard, bool canAddNewPlayers, bool isGameFinished, GuessResult guessResult) : base(id, currentCard, canAddNewPlayers, isGameFinished)
        {
            GuessResult = guessResult;
        }
    }
}
