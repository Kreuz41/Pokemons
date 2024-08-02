using Microsoft.EntityFrameworkCore;
using Pokemons.API.Middlewares;
using Pokemons.Core.Helpers;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.Core.Services.NotificationService;
using Pokemons.Core.Services.ReferralService;
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
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await service.Database.MigrateAsync();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapControllers();

        app.UseCors(options => options.WithOrigins("http://localhost:3001", "http://localhost:3002",
            "http://localhost:3000", "http://localhost:8080",
            "http://localhost:4200", "http://localhost:5173", "http://localhost:5010",
            "https://cix-lilac.vercel.app", "https://cix-nu.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

        app.UseMiddleware<AuthMiddleware>();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        //if (!app.Environment.IsDevelopment()) return;
        
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}