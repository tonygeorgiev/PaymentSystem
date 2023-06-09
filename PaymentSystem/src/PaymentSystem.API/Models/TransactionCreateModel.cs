﻿using PaymentSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.API.Models
{
    public class TransactionCreateModel
    {
        [Required]
        public Guid MerchantId { get; set; }
        public Guid? ReferencedTransactionId { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string CustomerEmail { get; set; }

        [Required, MaxLength(20)]
        [Phone]
        public string CustomerPhone { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public TransactionStatus TransactionStatus { get; set; }
    }
}
