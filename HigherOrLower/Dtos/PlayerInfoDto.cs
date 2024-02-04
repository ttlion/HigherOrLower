namespace HigherOrLower.Dtos
{
    public class PlayerInfoDto
    {
        public string Name { get; set; }

        public int Score { get; set; }
        
        public int OrderInGame { get; set; }
        
        public bool IsCurrentPlayerToMove { get; set; }

        public PlayerInfoDto(string name, int score, int orderInGame, bool isCurrentPlayerToMove)
        {
            Name = name;
            Score = score;
            OrderInGame = orderInGame;
            IsCurrentPlayerToMove = isCurrentPlayerToMove;
        }
    }
}
