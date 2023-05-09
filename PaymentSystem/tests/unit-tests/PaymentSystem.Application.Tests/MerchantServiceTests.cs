using AutoMapper;
using PaymentSystem.Application.DTOs;
using PaymentSystem.Application.Services;
using PaymentSystem.Domain.Models;
using PaymentSystem.Infrastructure.Repositories.Contracts;
using PaymentSystem.Application.Exceptions;
using System.Linq.Expressions;

namespace PaymentSystem.Application.Tests
{
    public class MerchantServiceTests
    {
        private readonly Mock<IMerchantRepository> _mockMerchantRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MerchantService _merchantService;

        public MerchantServiceTests()
        {
            _mockMerchantRepository = new Mock<IMerchantRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockMapper = new Mock<IMapper>();
            _merchantService = new MerchantService(_mockMerchantRepository.Object, _mockTransactionRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllMerchantsAsync_ShouldReturnAllMerchants()
        {
            // Arrange
            var merchants = new List<Merchant>
        {
            new Merchant { Id = Guid.NewGuid(), Name = "Merchant 1" },
            new Merchant { Id = Guid.NewGuid(), Name = "Merchant 2" },
            new Merchant { Id = Guid.NewGuid(), Name = "Merchant 3" }
        };
            _mockMerchantRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(merchants);

            // Act
            var result = await _merchantService.GetAllMerchantsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(merchants);
        }

        [Fact]
        public async Task GetMerchantByIdAsync_ShouldReturnMerchant_WhenMerchantExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var merchant = new Merchant { Id = id, Name = "Merchant" };
            _mockMerchantRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(merchant);

            // Act
            var result = await _merchantService.GetMerchantByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(merchant);
        }

        [Fact]
        public async Task GetMerchantByIdAsync_ShouldReturnNull_WhenMerchantDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockMerchantRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(null as Merchant);

            // Act
            var result = await _merchantService.GetMerchantByIdAsync(id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddMerchantAsync_WithValidDto_ShouldReturnMerchantId()
        {
            // Arrange
            var merchantCreateDto = new MerchantCreateDto("Test Merchant", "test@merchant.com", "Test Merchant Description", true);

            var expectedMerchantId = Guid.NewGuid();

            var mapperMock = new Mock<IMapper>();
            _mockMapper
                .Setup(m => m.Map<Merchant>(merchantCreateDto))
                .Returns(new Merchant { Id = expectedMerchantId });

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            _mockMerchantRepository.
                Setup(x => x.AddAsync(It.IsAny<Merchant>())).
                Returns(Task.CompletedTask).Verifiable();

            _mockMerchantRepository
                .Setup(r => r.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            var merchantService = new MerchantService(
                _mockMerchantRepository.Object,
                null, // Mocked ITransactionRepository not needed for this test
                _mockMapper.Object
            );

            // Act
            var result = await merchantService.AddMerchantAsync(merchantCreateDto);

            // Assert
            result.Should().Be(expectedMerchantId);

            merchantRepositoryMock.VerifyAll();
        }


        [Fact]
        public async Task UpdateMerchantAsync_Throws_EntityNotFoundException_When_Merchant_Is_Not_Found()
        {
            // Arrange
            Guid merchantId = Guid.NewGuid();
            MerchantUpdateDto merchantUpdateDto = new MerchantUpdateDto("test", "test", "test", true);

            _mockMerchantRepository
                .Setup(x => x.GetByIdAsync(merchantId))
                .ReturnsAsync((Merchant)null);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _merchantService.UpdateMerchantAsync(merchantId, merchantUpdateDto));

            _mockMerchantRepository.Verify(x => x.GetByIdAsync(merchantId), Times.Once);
            _mockMerchantRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateMerchantAsync_Updates_Merchant_Successfully()
        {
            // Arrange
            Guid merchantId = Guid.NewGuid();
            Merchant merchant = new Merchant
            {
                Id = merchantId,
                Name = "Test Merchant",
                Description = "Test Merchant Description",
                Email = "test@merchant.com",
                IsActive = true
            };
            MerchantUpdateDto merchantUpdateDto = new MerchantUpdateDto("New Name", "New Description", "newemail@merchant.com", false);

            _mockMerchantRepository
                .Setup(x => x.GetByIdAsync(merchantId))
                .ReturnsAsync(merchant);

            // Act
            await _merchantService.UpdateMerchantAsync(merchantId, merchantUpdateDto);

            // Assert
            _mockMerchantRepository.Verify(x => x.GetByIdAsync(merchantId), Times.Once);
            _mockMerchantRepository.Verify(x => x.Update(It.Is<Merchant>(m => m.Id == merchantId &&
                                                                             m.Name == merchantUpdateDto.Name &&
                                                                             m.Description == merchantUpdateDto.Description &&
                                                                             m.Email == merchantUpdateDto.Email &&
                                                                             m.IsActive == merchantUpdateDto.IsActive)), Times.Once);
            _mockMerchantRepository.Verify(x => x.SaveAsync(), Times.Once);
            _mockMerchantRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteMerchantAsync_WhenMerchantHasNoRelatedTransactions_DeletesMerchant()
        {
            // Arrange
            var merchantId = Guid.NewGuid();
            var merchant = new Merchant { Id = merchantId };
            _mockMerchantRepository.Setup(x => x.GetByIdAsync(merchantId)).ReturnsAsync(merchant);
            _mockTransactionRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(false);

            // Act
            await _merchantService.DeleteMerchantAsync(merchant);

            // Assert
            _mockMerchantRepository.Verify(x => x.Delete(merchant), Times.Once);
            _mockMerchantRepository.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMerchantAsync_WhenMerchantHasRelatedTransactions_ThrowsInvalidOperationException()
        {
            // Arrange
            var merchantId = Guid.NewGuid();
            var merchant = new Merchant { Id = merchantId };
            _mockMerchantRepository.Setup(x => x.GetByIdAsync(merchantId)).ReturnsAsync(merchant);
            _mockTransactionRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _merchantService.DeleteMerchantAsync(merchant));

            _mockMerchantRepository.Verify(x => x.Delete(merchant), Times.Never);
            _mockMerchantRepository.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateTransactionAsync_WithExistingTransaction_UpdatesTransaction()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockMapper = new Mock<IMapper>();

            var transactionService = new TransactionService(mockTransactionRepository.Object, null, mockMapper.Object);

            var transactionId = Guid.NewGuid();
            var transaction = new Transaction
            {
                Id = transactionId,
                MerchantId = Guid.NewGuid(),
                Amount = 100,
                CustomerEmail = "customer@test.com",
                CustomerPhone = "1234567890"
            };

            mockTransactionRepository.Setup(x => x.GetByIdAsync(transactionId)).ReturnsAsync(transaction);

            var transactionUpdateDto = new TransactionUpdateDto(200, "newcustomer@test.com", "9876543210");

            // Act
            await transactionService.UpdateTransactionAsync(transactionId, transactionUpdateDto);

            // Assert
            mockTransactionRepository.Verify(x => x.GetByIdAsync(transactionId), Times.Once);
            mockTransactionRepository.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Once);
            mockTransactionRepository.Verify(x => x.SaveAsync(), Times.Once);

            Assert.Equal(transactionUpdateDto.Amount, transaction.Amount);
            Assert.Equal(transactionUpdateDto.CustomerEmail, transaction.CustomerEmail);
            Assert.Equal(transactionUpdateDto.CustomerPhone, transaction.CustomerPhone);
        }

        [Fact]
        public async Task UpdateTransactionAsync_WithNonExistingTransaction_ThrowsArgumentException()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockMapper = new Mock<IMapper>();

            var transactionService = new TransactionService(mockTransactionRepository.Object, null, mockMapper.Object);

            var transactionId = Guid.NewGuid();

            var transactionUpdateDto = new TransactionUpdateDto(200, "newcustomer@test.com", "9876543210");


            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await transactionService.UpdateTransactionAsync(transactionId, transactionUpdateDto);
            });

            mockTransactionRepository.Verify(x => x.GetByIdAsync(transactionId), Times.Once);
            mockTransactionRepository.Verify(x => x.Update(It.IsAny<Transaction>()), Times.Never);
            mockTransactionRepository.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}