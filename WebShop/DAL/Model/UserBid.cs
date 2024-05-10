using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class UserBid
    {
        [Key]
        public int UserBidId { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int BidId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Bid Bid { get; set; }
    }

}
