using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(16)]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email addres")]
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        [Required]
        public UserRole Role { get; set; }

        // Navigation properties
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
    }

    public enum UserRole
    {
        User,
        Admin
    }
}
