using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.DTOs;

public record TransactionCreateDto(Guid MerchantId, decimal Amount, string CustomerEmail, string CustomerPhone);
