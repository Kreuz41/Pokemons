using Microsoft.AspNetCore.Http.HttpResults;
using Pokemons.Core.Services.PlayerService;

namespace Pokemons.API.Middlewares;

public class AuthMiddleware : IMiddleware
{
    public AuthMiddleware(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    private readonly IPlayerService _playerService;
    
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

        if (!await _playerService.IsPlayerExist(id))
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(
                CallResult.CallResult<bool>.Failure($"User does not exist"));
            return;
        }
        context.Items.Add("UserId", id);
        
        await next(context);
    }
}