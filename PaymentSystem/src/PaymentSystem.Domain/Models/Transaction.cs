using PaymentSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PaymentSystem.Domain.Models;
public class Transaction : Entity<Guid>
{
    public Transaction()
    {
        this.Id= Guid.NewGuid();
    }

    [Required]
    public Guid TransactionGuid { get; set; }

    [Required, Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public TransactionStatus Status { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string CustomerEmail { get; set; }

    [Required, MaxLength(20)]
    public string CustomerPhone { get; set; }

    public int MerchantId { get; set; }
    public Merchant Merchant { get; set; }
}