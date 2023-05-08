using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public MerchantController(IMerchantService merchantService, IMapper mapper)
        {
            _merchantService = merchantService;
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
                return NotFound();
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
                return NotFound();
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
    }
}
