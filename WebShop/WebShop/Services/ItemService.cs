using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Models.ViewModel;

namespace WebShop.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IArtistRepository artistRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _artistRepository = artistRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IEnumerable<CreateItemViewModel> GetAllItems()
        {
            var items = _itemRepository.GetAll();
            return _mapper.Map<List<CreateItemViewModel>>(items);
        }

        public CreateItemViewModel GetItemById(int id)
        {
            var item = _itemRepository.GetById(id);
            return _mapper.Map<CreateItemViewModel>(item);
        }

        public void CreateItem(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel);
            _itemRepository.Add(item);
        }

        public void UpdateItem(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel);
            _itemRepository.Update(item);
        }

        public void DeleteItem(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item != null)
            {
                _itemRepository.Delete(item);
            }
        }

        public IEnumerable<SelectListItem> GetAllArtists()
        {
            var artists = _artistRepository.GetAll() ?? new List<Artist>();
            return artists.Select(a => new SelectListItem { Value = a.ArtistId.ToString(), Text = a.Name }).ToList();
        }

        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll() ?? new List<Category>();
            return categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();
        }
    }
}
