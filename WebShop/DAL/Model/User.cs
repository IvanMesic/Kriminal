using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }

        //Or just password doesnt really matter
        public string PasswordSalt { get; set; }
        public String PasswordHash {  get; set; }

        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserBid> UserBids { get; set; }
    }
}
