using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Pokemons.Core.BackgroundServices.CacheCollector;
using Pokemons.Core.Helpers;
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
            if (!context.Request.Path.ToString().EndsWith("createUser"))
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(
                    CallResult.CallResult<bool>.Failure($"User does not exist"));
                return;
            }
        }
        
        var bearerToken = context.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(bearerToken))
        {
            var handler = new JwtSecurityTokenHandler();
            var token = authorizationHeader.ToString().Replace("Bearer ", "");
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                if (userIdClaim != null)
                {
                    if (!long.TryParse(userIdClaim.Value, out var hashId) || hashId != id)
                    {
                        await context.Response.WriteAsJsonAsync(
                            CallResult.CallResult<bool>.Failure($"Invalid access token"));
                        return;
                    }
                    context.Items.Add("UserId", userIdClaim.Value);
                }
            }
        }

        CacheCollectorService.UpdateSession(id);
        
        await next(context);
    }
}