namespace HigherOrLower.Documentation
{
    public static class GameIntro
    {
        public const string GameTitle = "Higher or Lower Game via API calls";

        public const string GameDescription= @"
<h3>Web API that allows playing ""Higher or Lower"" game without a deck of cards</h3>
<h4>Rules of the game</h4>
<p>The dealer shuffles a deck of 52 cards, draws the first card, and places it on the table.</p>
<p>The first player guesses if the next card will be higher or lower than the one on the table. The player wins that play if he guessed correctly (if the card had the same face value, it counts as a win).</p>
<p>Move on to the next player, clockwise, until all cards of the deck have been drawn.</p>
";

    }
}
