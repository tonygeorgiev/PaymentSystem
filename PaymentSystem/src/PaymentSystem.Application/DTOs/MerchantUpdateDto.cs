namespace PaymentSystem.Application.DTOs;

public record MerchantUpdateDto(string Name, string Description, string Email, bool IsActive);