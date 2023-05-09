using FluentAssertions;
using PaymentSystem.API.Models;
using PaymentSystem.API.Tests.Factory;
using PaymentSystem.Domain.Models;
using System.Net;
using System.Net.Http.Json;

namespace PaymentSystem.API.Tests
{
    [Collection("IntegrationTests")]
    public class MerchantControllerTests
    {
        private readonly TestWebApplicationFactory<Startup> _factory;

        public MerchantControllerTests(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/merchant");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_WithExistingId_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var merchant = new MerchantCreateModel
            {
                Name = "Test Merchant",
                Email = "test@test.com",
                Description = "Test merchant description"
            };
            var createResponse = await client.PostAsJsonAsync("/api/merchant", merchant);
            var createdMerchant = await createResponse.Content.ReadFromJsonAsync<Merchant>();

            // Act
            var response = await client.GetAsync($"/api/merchant/{createdMerchant.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<Merchant>();
            result.Should().NotBeNull();
            result.Id.Should().Be(createdMerchant.Id);
            result.Name.Should().Be(merchant.Name);
            result.Email.Should().Be(merchant.Email);
            result.Description.Should().Be(merchant.Description);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await client.GetAsync($"/api/merchant/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

}