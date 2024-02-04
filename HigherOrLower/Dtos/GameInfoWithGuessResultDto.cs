using HigherOrLower.Utils.Enums;

namespace HigherOrLower.Dtos
{
    public class GameInfoWithGuessResultDto : GameInfoDto
    {
        public GuessResult GuessResult { get; }

        public GameInfoWithGuessResultDto() : base() 
        {
            
        }

        public GameInfoWithGuessResultDto(int id, string currentCard, bool isGameFinished, GuessResult guessResult) : base(id, currentCard, isGameFinished)
        {
            GuessResult = guessResult;
        }
    }
}
