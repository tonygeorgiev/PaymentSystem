namespace PaymentSystem.Application.DTOs;

public record TransactionUpdateDto(decimal Amount, string CustomerEmail, string CustomerPhone);
