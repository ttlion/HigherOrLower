using Microsoft.EntityFrameworkCore.Migrations;
using System.Net.NetworkInformation;

#nullable disable

namespace HigherOrLower.Migrations
{
    /// <inheritdoc />
    public partial class HigherOrLower_FillCardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var suits = new[] 
            { 
                "diamonds", 
                "clubs", 
                "hearts",
                "spades" 
            };

            var cardsNames = new[] {
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "jack",
                "queen",
                "king",
                "ace",
            };

            var insertsForAllCards = new List<string>();

            for (var suitId = 0; suitId < suits.Length; suitId++)
            {
                var suitName = suits[suitId];

                for (var nameId = 0; nameId < cardsNames.Length; nameId++)
                {
                    var cardName = cardsNames[nameId];

                    var cardId = cardsNames.Length * suitId + nameId;
                    var cardNameWithSuitName = $"{cardName} of {suitName}";
                    var cardValue = nameId + 2;

                    insertsForAllCards.Add($"({cardId}, '{cardNameWithSuitName}', {cardValue})");
                }
            }

            migrationBuilder.Sql($"INSERT INTO Cards(Id, Name, Value) VALUES {string.Join(",", insertsForAllCards)};");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
