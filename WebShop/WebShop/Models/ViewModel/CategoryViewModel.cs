using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.ViewModel
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
