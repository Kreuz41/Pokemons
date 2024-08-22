using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pokemons.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/aractivity")]
    public class ArActivityController : ControllerBase
    {
        private readonly IArActivityService _arActivityService;

        public ArActivityController(IArActivityService arActivityService)
        {
            _arActivityService = arActivityService;
        }

        [HttpPost("coin")]
        public async Task<IActionResult> CollectCoin()
        {
            var playerId = (long)HttpContext.Items["UserId"]!;

            var (success, activityDto) = await _arActivityService.CollectCoinAsync(playerId);

            if (success)
            {
                return Ok(new
                {
                    Message = "Монетка успешно собрана!",
                    Activity = activityDto
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Не удалось собрать монетку. Возможно, недостаточно энергии или игрок не найден."
                });
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetActivity()
        {
            var playerId = (long)HttpContext.Items["UserId"]!;
            var arActivityDto = await _arActivityService.GetArActivityAsync(playerId);

            if (arActivityDto == null)
            {
                return NotFound(new { Message = "Активность игрока не найдена." });
            }

            return Ok(arActivityDto);
        }
    }
}
