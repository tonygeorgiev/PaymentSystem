using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Domain.Models;
public enum TransactionType
{
    Authorize,
    Charge,
    Refund,
    Reversal
}