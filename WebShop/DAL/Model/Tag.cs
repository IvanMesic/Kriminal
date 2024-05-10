using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<ItemTag> ItemTags { get; set; }
        public virtual ICollection<ArtistTag> ArtistTags { get; set; }
    }
}
