using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;

namespace PaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var merchants = await _merchantService.GetAllMerchantsAsync();
            return Ok(merchants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);

            if (merchant == null)
            {
                return NotFound();
            }

            return Ok(merchant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Merchant merchant)
        {
            await _merchantService.AddMerchantAsync(merchant);
            return CreatedAtAction(nameof(GetById), new { id = merchant.Id }, merchant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Merchant merchant)
        {
            if (id != merchant.Id)
            {
                return BadRequest();
            }

            await _merchantService.UpdateMerchantAsync(merchant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);

            if (merchant == null)
            {
                return NotFound();
            }

            await _merchantService.DeleteMerchantAsync(merchant);
            return NoContent();
        }
    }


}
