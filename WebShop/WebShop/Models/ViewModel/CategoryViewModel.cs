using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.ViewModel
{
    public class CategoryViewModel
    {
        [Key]
        public string Name { get; set; }

        public virtual ICollection<CreateItemViewModel> Items { get; set; }
    }
}
