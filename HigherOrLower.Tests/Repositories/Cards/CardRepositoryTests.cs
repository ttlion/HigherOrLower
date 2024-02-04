using FluentAssertions;
using HigherOrLower.Entities.Cards;
using HigherOrLower.Infrastructure.Database;
using HigherOrLower.Repositories.Cards;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace HigherOrLower.Tests.Repositories.Cards
{
    public class CardRepositoryTests
    {
        private static readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    { "DatabaseType", "InMemory" },
                    { "InMemoryDatabaseName", $"Db{nameof(CardRepositoryTests)}" }
                })
            .Build();

        static CardRepositoryTests()
        {
            var higherOrLowerDbContext = new HigherOrLowerDbContext(_configuration);
            higherOrLowerDbContext.Cards.Add(new Card() { Id = 1, Name = "10 of diamonds", Value = 10 });
            higherOrLowerDbContext.Cards.Add(new Card() { Id = 2, Name = "queen of hearts", Value = 12 });
            higherOrLowerDbContext.Cards.Add(new Card() { Id = 3, Name = "ace of spades", Value = 13 });
            higherOrLowerDbContext.SaveChanges();
        }

        private readonly ICardRepository _cardRepository;

        public CardRepositoryTests()
        {
            _cardRepository = new CardRepository(new HigherOrLowerDbContext(_configuration));
        }

        [Fact]
        public void GetTotalNumberOfCardsTest()
        {
            _cardRepository.GetTotalNumberOfCards().Should().Be(3);
        }

        [Theory]
        [InlineData(1, "10 of diamonds", 10)]
        [InlineData(2, "queen of hearts", 12)]
        [InlineData(3, "ace of spades", 13)]
        public void GetCardTest(int id, string expectedName, int expectedValue)
        {
            var card = _cardRepository.GetCard(id);
            card.Id.Should().Be(id);
            card.Name.Should().Be(expectedName);
            card.Value.Should().Be(expectedValue);
        }

        [Fact]
        public void GetCardsTest()
        {
            var ids = new List<int> { 1, 3 };
            var names = new Dictionary<int, string>
            {
                {1, "10 of diamonds" },
                {3, "ace of spades" },
            };

            var cards = _cardRepository.GetCards(ids);

            cards.Count.Should().Be(ids.Count);
            ids.ForEach(id => cards[id].Name.Should().Be(names[id]));
        }
    }
}
