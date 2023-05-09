using PaymentSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations;

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

        public decimal TotalTransactionSum 
        { 
            get
            {
                return this.GetTotalTransactionSum();
            }
            set
            {
                this.TotalTransactionSum = value;
            }
        }

        public ICollection<Transaction> Transactions { get; set; }

        public decimal GetApprovedChargeTransactionSum()
        {
            return Transactions
                .Where(t => t.Status == TransactionStatus.Approved && t.TransactionType == TransactionType.Charge)
                .Sum(t => t.Amount);
        }

        public decimal GetApprovedRefundTransactionSum()
        {
            return Transactions
                .Where(t => t.Status == TransactionStatus.Approved && t.TransactionType == TransactionType.Refund)
                .Sum(t => t.Amount);
        }

        public decimal GetTotalTransactionSum()
        {
            return GetApprovedChargeTransactionSum() - GetApprovedRefundTransactionSum();
        }
    }
}
