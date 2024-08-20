using Microsoft.AspNetCore.Mvc;
using payment.API.Dtos;
using payment.API.Services;

namespace payment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController(IPaymentService _paymentService) : ControllerBase
    {
        [HttpGet("accountInfo/{accountId}")]
        public IActionResult AccountInfo(Guid accountId)
        {
            var data = _paymentService.GetAccountInfo(accountId);
            return data is not default(AccountDto) ?  Ok(data) : NotFound("Account not found!");
        }

        [HttpPut("transfer/{fromAccountId}")]
        public async Task<IActionResult> TransferMoneyAsync(Guid fromAccountId, TransferDto transferDetail)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _paymentService.TransferAmount(fromAccountId, transferDetail));
        }

        [HttpPost("seedAccounts")]
        public IActionResult Seed() => Ok(_paymentService.SeedAccount());

        [HttpGet("allAccount")]
        public IActionResult AllAccount() => Ok(_paymentService.GetAccounts());

    }
}
