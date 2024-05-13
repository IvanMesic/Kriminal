using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImagePath { get; set; }

        // Foreign keys
        public int ArtistId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        [ForeignKey("ArtistId")]
        public virtual Artist Artist { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
        public virtual ICollection<ItemTag> ItemTags { get; set; }

    }

}
