using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.API.Common.Exceotions;
using PaymentSystem.API.Models;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace PaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public MerchantController(
            IMerchantService merchantService, 
            ITransactionService transactionService,
            IMapper mapper)
        {
            _merchantService = merchantService;
            _transactionService = transactionService;
            _mapper = mapper;
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
                throw new NotFoundException($"Merchant with Id: {id} was not found.");
            }

            return Ok(merchant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MerchantCreateModel merchant)
        {
            var id = await _merchantService.AddMerchantAsync(_mapper.Map<MerchantCreateDto>(merchant));
            return CreatedAtAction(nameof(GetById), new { id = id}, merchant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MerchantUpdateModel merchant)
        {
            await _merchantService.UpdateMerchantAsync(id, _mapper.Map<MerchantUpdateDto>(merchant));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(id);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant with Id: {id} was not found.");
            }

            await _merchantService.DeleteMerchantAsync(merchant);
            return NoContent();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportMerchantsCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided");
            }
            
            await _merchantService.CreateMerchantsFromCsvAsync(file.OpenReadStream());

            return Ok();
        }

        [HttpPost("{merchantId}/payments")]
        public async Task<IActionResult> ProcessPayment(Guid merchantId, [FromBody] PaymentModel paymentModel)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(merchantId);

            if (merchant == null)
            {
                throw new NotFoundException($"Merchant with Id: {merchantId} was not found.");
            }

            if (!merchant.IsActive)
            {
                return BadRequest("Merchant is not active");
            }

            var transactionDto = _mapper.Map<TransactionCreateDto>(paymentModel);
            transactionDto.TransactionType = TransactionType.Charge;
            await _transactionService.AddTransactionAsync(transactionDto);

            return Ok();
        }
    }
}
