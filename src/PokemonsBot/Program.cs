using PokemonsBot.Bot;

var builder = WebApplication.CreateBuilder();
builder.Environment.EnvironmentName = Environments.Development;

_ = new PokemonBot(builder.Configuration["Bot:Token"] 
                         ?? throw new NullReferenceException());

var app = builder.Build();
app.Run();