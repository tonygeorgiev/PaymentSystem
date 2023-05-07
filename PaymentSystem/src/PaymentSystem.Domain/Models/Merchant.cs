using PaymentSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PaymentSystem.Domain.Models
{
    public class Merchant : Entity<Guid>
    {
        public Merchant()
        {
            this.Id = Guid.NewGuid();
        }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public decimal TotalTransactionSum { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
