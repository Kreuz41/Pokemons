using Microsoft.AspNetCore.Http.HttpResults;

namespace Pokemons.API.Middlewares;

public class AuthMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var header = context.Request.Headers["userId"];
        if (header.Count == 0)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(
                CallResult.CallResult<bool>.Failure("UserId not found"));
            return;
        }
        
        var isSuccess = long.TryParse(header, out var id);
        if (!isSuccess)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(
                CallResult.CallResult<bool>.Failure($"Invalid UserId: {header}"));
            return;
        }
        
        context.Items.Add("UserId", id);
        
        await next(context);
    }
}