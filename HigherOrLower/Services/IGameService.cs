using Microsoft.AspNetCore.Mvc;

namespace HigherOrLower.Services
{
    public interface IGameService
    {
        JsonResult? TryCreateNewGame();
    }
}