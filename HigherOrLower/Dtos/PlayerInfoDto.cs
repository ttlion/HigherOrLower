namespace HigherOrLower.Dtos
{
    public class PlayerInfoDto
    {
        public string Name { get; }

        public int Score { get; }
        
        public int OrderInGame { get; }
        
        public bool IsCurrentPlayerToMove { get; }

        public PlayerInfoDto(string name, int score, int orderInGame, bool isCurrentPlayerToMove)
        {
            Name = name;
            Score = score;
            OrderInGame = orderInGame;
            IsCurrentPlayerToMove = isCurrentPlayerToMove;
        }
    }
}
