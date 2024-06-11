using DAL.Model;
using WebShop.Model;

namespace WebShop.Services.Interfaces
{
    public interface IItemService
    {
        Task<string> UploadImageAsync(IFormFile file);
        (IList<Item> items, int totalPages) GetFilteredItems(ItemListViewModel itemListViewModel);
        Item GetItemById(int id);
        void CreateItem(CreateItemViewModel itemViewModel);
        CreateItemViewModel GetEditItemViewModel(int id);
        void EditItem(CreateItemViewModel itemViewModel);
        void DeleteItem(int id);
        IEnumerable<Item> GetAllItemsForUser(int userId);
        IEnumerable<Bid> GetHighestBidsForUser(int userId);
    }
}
