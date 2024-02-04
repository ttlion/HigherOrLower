using HigherOrLower.Utils.Enums;

namespace HigherOrLower.Dtos
{
    public class GameWithNextCardAndGuessResultDto : GameWithNextCardDto
    {
        public GuessResult GuessResult { get; }

        public GameWithNextCardAndGuessResultDto() : base() { }

        public GameWithNextCardAndGuessResultDto(int id, string nextCard, bool isLastCard, GuessResult guessResult) : base(id, nextCard, isLastCard)
        {
            GuessResult = guessResult;
        }
    }
}
