using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImagePath { get; set; }

        // Navigation properties
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ArtistTag> ArtistTags { get; set; }
    }
}
