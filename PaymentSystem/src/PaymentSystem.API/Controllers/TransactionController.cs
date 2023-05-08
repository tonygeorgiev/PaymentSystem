using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.API.Models;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Services.Contracts;
using PaymentSystem.Domain.Models;

namespace PaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionCreateModel transaction)
        {
            var id = await _transactionService.AddTransactionAsync(_mapper.Map<TransactionCreateDto>(transaction));
            return CreatedAtAction(nameof(GetById), new { id = id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TransactionUpdateModel transaction)
        {
            await _transactionService.UpdateTransactionAsync(id, _mapper.Map<TransactionUpdateDto>(transaction));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            await _transactionService.DeleteTransactionAsync(transaction);
            return NoContent();
        }

        [HttpPost("delete-old-transactions")]
        public async Task<ActionResult> DeleteOldTransactions()
        {
            await _transactionService.DeleteOldTransactions();
            return Ok();
        }
    }
}
