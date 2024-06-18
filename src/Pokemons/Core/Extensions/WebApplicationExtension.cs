using Pokemons.API.Middlewares;

namespace Pokemons.Core.Extensions;

public static class WebApplicationExtension
{
    public static void Configure(this WebApplication app)
    {
        ConfigureSwagger(app);
        ConfigurePipeline(app);
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