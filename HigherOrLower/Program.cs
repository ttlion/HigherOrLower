using HigherOrLower.Engines;
using HigherOrLower.Infrastructure.Database;
using HigherOrLower.Repositories.Cards;
using HigherOrLower.Repositories.Games;
using HigherOrLower.Services;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "aaa",
        Description = "bb"
    });
});

builder.Services.AddScoped<IGameEngine, GameEngine>();
builder.Services.AddScoped<IHigherOrLowerDbContext, HigherOrLowerDbContext>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
