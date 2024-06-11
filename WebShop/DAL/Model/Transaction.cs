using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign keys
        public int UserId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
    }

  
}
