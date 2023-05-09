using AutoMapper;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Services;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Repositories.Contracts;

namespace PaymentSystem.Application.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<IMerchantRepository> _mockMerchantRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockMerchantRepository = new Mock<IMerchantRepository>();
            _mockMapper = new Mock<IMapper>();
            _transactionService = new TransactionService(_mockTransactionRepository.Object, _mockMerchantRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnListOfTransactions()
        {
            // Arrange
            var expectedTransactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Amount = 10 },
            new Transaction { Id = Guid.NewGuid(), Amount = 20 },
            new Transaction { Id = Guid.NewGuid(), Amount = 30 }
        };

            _mockTransactionRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedTransactions);

            // Act
            var result = await _transactionService.GetAllTransactionsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(expectedTransactions.Count);
            result.Should().BeEquivalentTo(expectedTransactions);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedTransaction = new Transaction { Id = id, Amount = 50 };

            _mockTransactionRepository.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(expectedTransaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedTransaction);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnNull_WhenTransactionDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            Transaction expectedTransaction = null;

            _mockTransactionRepository.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(expectedTransaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddTransactionAsync_WhenMerchantNotFound_ThrowsArgumentException()
        {
            // Arrange
            var transactionCreateDto = new TransactionCreateDto
            {
                MerchantId = Guid.NewGuid()
            };

            _mockMerchantRepository.Setup(x => x.GetByIdAsync(transactionCreateDto.MerchantId)).ReturnsAsync((Merchant)null);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _transactionService.AddTransactionAsync(transactionCreateDto));
            _mockTransactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Never);
            _mockTransactionRepository.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task AddTransactionAsync_WithNonExistentMerchant_ThrowsArgumentException()
        {
            // Arrange
            var mockMerchantRepository = new Mock<IMerchantRepository>();
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockMapper = new Mock<IMapper>();

            var transactionService = new TransactionService(mockTransactionRepository.Object, mockMerchantRepository.Object, mockMapper.Object);

            var transactionCreateDto = new TransactionCreateDto
            {
                MerchantId = Guid.NewGuid(),
                Amount = 100,
                TransactionType = TransactionType.Authorize
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => transactionService.AddTransactionAsync(transactionCreateDto));
        }
    }

}
