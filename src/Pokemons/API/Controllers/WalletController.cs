using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemons.Services;

namespace Pokemons.API.Controllers
{
    [ApiController]
     [Authorize]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        
       
        [HttpGet()]
        public async Task<IActionResult> GetWallet()
        {
            var playerId = (long)HttpContext.Items["UserId"]!;

            var wallet = await _walletService.GetWalletByPlayerIdAsync(playerId);
            if (wallet == null)
            {
                return NotFound(new { message = "Wallet not found" });
            }
            return Ok(wallet);
        }
  

        // Снять TRX с кошелька
        [HttpPost("withdrawTrx")]
        public async Task<IActionResult> WithdrawTrx([FromBody] int amount)
        {
            var playerId = (long)HttpContext.Items["UserId"]!;

            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            var success = await _walletService.WithdrawTrxAsync(playerId, amount);
            if (!success)
            {
                return BadRequest("Insufficient funds or wallet not found");
            }

           return Ok(new { status = true });
        }
        
        [AllowAnonymous] 
        
        [HttpPost("depositUsdt")]
        public async Task<IActionResult> DepositUsdt([FromBody] DepositRequestDto requestDto)
        {
            
            if (requestDto.Amount <= 0)
            {
                return BadRequest("Amount must be greater than zero");
            }

            await _walletService.DepositUsdtToWalletAsync(requestDto.Payload.PlayerId, requestDto.Amount);
            return Ok(new { status = true });
        }
         [HttpPost("invoice")]

    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequestDto requestDto)
    {

        try
        {
            var playerId = (long)HttpContext.Items["UserId"]!;
            var payUrl = await _walletService.CreateInvoiceAsync(playerId, requestDto.Amount);
            var responseDto = new CreateInvoiceResponseDto { PayUrl = payUrl };
            return Ok(responseDto);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, "Error while creating invoice: " + ex.Message);
        }
    }
    }
}
