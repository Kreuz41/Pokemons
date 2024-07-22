using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/notifications/")]
public class NotificationController : ControllerBase
{
    public NotificationController(INotificationHandler handler)
    {
        _handler = handler;
    }

    private readonly INotificationHandler _handler;

    [HttpGet("getNotifications")]
    public async Task<IResult> GetUnreadNotifications([FromQuery] int offset)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetNotifications(playerId, offset);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPost("readNotification")]
    public async Task<IResult> ReadNotification([FromBody] Dictionary<string, long> requestId)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        if (requestId.TryGetValue("id", out var notificationId))
        {
            var result = await _handler.ReadNotification(playerId, notificationId);
            return result.Status ? Results.Ok(result) : Results.BadRequest(result);
        }

        return Results.BadRequest(CallResult.CallResult<bool>.Failure("Param id not found"));
    }
    
    [HttpPost("readAllNotifications")]
    public async Task<IResult> ReadAllNotifications()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.ReadAllNotifications(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPost("deleteNotification")]
    public async Task<IResult> DeleteNotification([FromQuery] long notificationId)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.DeleteNotification(playerId, notificationId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPost("deleteAllNotifications")]
    public async Task<IResult> DeleteAllNotifications()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.DeleteAllNotifications(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpGet("getNews")]
    public async Task<IResult> GetUnreadNews([FromQuery] int offset)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetNews(playerId, offset);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPost("readNews")]
    public async Task<IResult> ReadNews([FromBody] long newsId)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.ReadNews(playerId, newsId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPost("readAllNews")]
    public async Task<IResult> ReadAllNews()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.ReadAllNews(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}