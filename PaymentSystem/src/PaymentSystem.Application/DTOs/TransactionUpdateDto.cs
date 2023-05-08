using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.DTOs;

public record TransactionUpdateDto(decimal Amount, string CustomerEmail, string CustomerPhone);
