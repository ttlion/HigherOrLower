using HigherOrLower.Engines;
using HigherOrLower.Infrastructure.Database;
using HigherOrLower.Repositories.Cards;
using HigherOrLower.Repositories.Games;
using HigherOrLower.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGameEngine, GameEngine>();
builder.Services.AddScoped<IHigherOrLowerDbContext, HigherOrLowerDbContext>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
