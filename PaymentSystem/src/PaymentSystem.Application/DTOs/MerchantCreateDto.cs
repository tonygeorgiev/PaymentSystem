namespace PaymentSystem.Application.DTOs
{
    public record MerchantCreateDto(string Name, string Description, string Email, bool IsActive);
}
