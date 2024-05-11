using DAL.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.ViewModel
{
    public class ItemViewModel
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImagePath { get; set; }

        public ArtistViewModel Artist { get; set; }

        public CategoryViewModel Category { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
        public virtual ICollection<ItemTag> ItemTags { get; set; }
    }
}