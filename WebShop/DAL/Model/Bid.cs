using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }

        // Foreign keys
        public int ItemId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public virtual Item Item { get; set; }
        public virtual User User{ get; set; }

       
    }
}
