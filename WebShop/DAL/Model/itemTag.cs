using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class ItemTag
    {
        [Key]
        public int ItemTagId { get; set; }

        // Foreign keys
        public int ItemId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public virtual Item Item { get; set; }
        public virtual Tag Tag { get; set; }

    
    }
}
