using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Model;
using WebShop.Services.Interfaces;

namespace WebShop.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;

        public ItemService(
            IItemRepository itemRepository,
            ITagRepository tagRepository,
            IItemTagRepository itemTagRepository,
            ICategoryRepository categoryRepository,
            IArtistRepository artistRepository,
            IBidRepository bidRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _tagRepository = tagRepository;
            _itemTagRepository = itemTagRepository;
            _categoryRepository = categoryRepository;
            _artistRepository = artistRepository;
            _bidRepository = bidRepository;
            _mapper = mapper;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file selected.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/static-files", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/static-files/" + file.FileName;
        }

        public (IList<Item> items, int totalPages) GetFilteredItems(ItemListViewModel itemListViewModel)
        {
            var filteredItems = _itemRepository.GetFiltered(
                tags: itemListViewModel.tags,
                artists: itemListViewModel.artists,
                categories: itemListViewModel.categories,
                priceMin: itemListViewModel.priceMin,
                priceMax: itemListViewModel.priceMax,
                searchQuery: itemListViewModel.searchQuery,
                includeSold: itemListViewModel.includeSold,
                includeSale: itemListViewModel.includeSale
            );

            int totalPages = (int)Math.Ceiling((double)filteredItems.Count() / 10);
            return (filteredItems, totalPages);
        }

        public Item GetItemById(int id)
        {
            return _itemRepository.GetById(id);
        }

        public void CreateItem(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Add(item);

            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }

            foreach (var newTag in itemViewModel.newTags)
            {
                var existingTag = _tagRepository.GetByName(newTag);

                if (existingTag == null)
                {
                    var tag = new Tag { Name = newTag };
                    _tagRepository.Add(tag);
                    existingTag = tag;
                }

                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = existingTag.TagId };
                _itemTagRepository.Add(itemTag);
            }
        }

        public CreateItemViewModel GetEditItemViewModel(int id)
        {
            var item = _itemRepository.GetById(id);
            if (item == null) return null;

            var itemVM = new CreateItemViewModel { item = item, tagIds = new List<int>() };
            foreach (ItemTag itemTag in item.ItemTags)
            {
                itemVM.tagIds.Add(itemTag.TagId);
            }

            return itemVM;
        }

        public void EditItem(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Update(item);

            int id = item.ItemId;
            item = _itemRepository.GetById(id);

            foreach (ItemTag itemTag in item.ItemTags)
            {
                _itemTagRepository.Delete(itemTag);
            }

            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }
        }

        public void DeleteItem(int id)
        {
            var item = _itemRepository.GetById(id);
            _itemRepository.Delete(item);
        }

        public IEnumerable<Item> GetAllItemsForUser(int userId)
        {
            return _itemRepository.GetAllItemsForUser(userId);
        }

        public IEnumerable<Bid> GetHighestBidsForUser(int userId)
        {
            return _bidRepository.GetHighestBidsForUser(userId);
        }
    }
}
