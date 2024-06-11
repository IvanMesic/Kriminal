using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class TransactionItem
    {
        [Key]
        public int TransactionItemId { get; set; }

        // Foreign keys
        public int TransactionId { get; set; }
        public int ItemId { get; set; }

        // Navigation properties
        public virtual Transaction Transaction { get; set; }
        public virtual Item Item { get; set; }

      
    }

 
}
