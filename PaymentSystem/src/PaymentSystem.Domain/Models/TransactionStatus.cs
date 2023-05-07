using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Domain.Models;
public enum TransactionStatus
{
    Approved,
    Reversed,
    Refunded,
    Error
}
