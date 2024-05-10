using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class ArtistTag
    {
        [Key]
        public int ArtistTagId { get; set; }

        // Foreign keys
        public int ArtistId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public virtual Artist Artist { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
