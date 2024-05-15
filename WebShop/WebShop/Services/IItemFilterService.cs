using DAL.Model.DTO;
using DAL.Model;
using WebShop.Model;

namespace WebShop.Services
{
    public interface IItemFilterService
    {
        int getTotalPages();
        IList<Item> GetFilteredItems(ItemFilterViewModel filterModel);
        ItemFilterViewModel GetFilterOptions();
    }
}
