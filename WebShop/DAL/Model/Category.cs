using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<Item> Items { get; set; }
    }
}
