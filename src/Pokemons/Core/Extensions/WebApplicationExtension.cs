using Microsoft.EntityFrameworkCore;
using Pokemons.API.Middlewares;
using Pokemons.DataLayer.Database;

namespace Pokemons.Core.Extensions;

public static class WebApplicationExtension
{
    public static void Configure(this WebApplication app)
    {
        ConfigureSwagger(app);
        ConfigureDatabase(app);
        ConfigurePipeline(app);
    }

    private static async void ConfigureDatabase(WebApplication app)
    {
        var service = app.Services.GetRequiredService<AppDbContext>();
        await service.Database.MigrateAsync();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseMiddleware<AuthMiddleware>();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}