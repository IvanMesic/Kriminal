using DAL.Model.DTO;
using DAL.Model;

namespace WebShop.Model
{
    public class ItemListViewModel
    {
        public IList<Item> Items { get; set; }
        public ItemFilterViewModel Filter { get; set; }

        public int TotalPages { get; set; } 
    }
}
