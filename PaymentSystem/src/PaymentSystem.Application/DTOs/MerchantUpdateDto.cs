using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.DTOs;

public record MerchantUpdateDto(string Name, string Description, string Email, bool IsActive);