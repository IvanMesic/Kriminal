using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Models.ViewModel;

namespace WebShop.Services
{
    public interface IItemService
    {
        IEnumerable<CreateItemViewModel> GetAllItems();
        CreateItemViewModel GetItemById(int id);
        void CreateItem(CreateItemViewModel itemViewModel);
        void UpdateItem(CreateItemViewModel itemViewModel);
        void DeleteItem(int id);
        IEnumerable<SelectListItem> GetAllArtists();
        IEnumerable<SelectListItem> GetAllCategories();
    }
}
