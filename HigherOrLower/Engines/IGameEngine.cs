using HigherOrLower.Dtos;

namespace HigherOrLower.Engines
{
    public interface IGameEngine
    {
        GameWithNextCardDto? TryCreateNewGame();

        int? TryDrawNextGameCard(Guid gameId);
    }
}