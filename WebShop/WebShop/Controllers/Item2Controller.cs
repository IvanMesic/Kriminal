using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using WebShop.Model;
using WebShop.Services;

namespace WebShop.Controllers
{
    public class Item2Controller : Controller
    {
        private readonly IItemFilterService _itemFilterService;
        private readonly IItemRepository _itemRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly IMapper _mapper;

        public Item2Controller(IItemFilterService itemFilterService, IItemRepository itemRepository,
                              IItemTagRepository itemTagRepository, IMapper mapper)
        {
            _itemFilterService = itemFilterService;
            _itemRepository = itemRepository;
            _itemTagRepository = itemTagRepository;
            _mapper = mapper;
        }

        public IActionResult Index(ItemFilterViewModel filterModel)
        {
            // Ensure PageNumber and PageSize have valid values
            filterModel.PageNumber = filterModel.PageNumber < 1 ? 1 : filterModel.PageNumber;
            filterModel.PageSize = filterModel.PageSize < 1 ? 10 : filterModel.PageSize;



            var filteredItems = _itemFilterService.GetFilteredItems(filterModel);

            int totalPages = _itemFilterService.getTotalPages();

            IList<Item> items2 = _itemRepository.GetFiltered(tags: filterModel.SelectedTags.Select(tagId => new Tag { TagId = int.Parse(tagId) }).ToList(), artists: filterModel.SelectedArtists.Select(artistId => new Artist { ArtistId = int.Parse(artistId) }).ToList(), categories: filterModel.SelectedCategories.Select(categoryId => new Category { CategoryId = int.Parse(categoryId) }).ToList(), priceMax: filterModel.PriceMax, searchQuery: filterModel.SearchQuery, pageNum: null, pageSize: null);


            var filterOptions = _itemFilterService.GetFilterOptions();
            filterModel.Categories = filterOptions.Categories;
            filterModel.Artists = filterOptions.Artists;
            filterModel.Tags = filterOptions.Tags;

            var itemViewModel = new ItemListViewModel
            {
                Items = filteredItems,
                Filter = filterModel,
                TotalPages = totalPages,
                CurrentPage = filterModel.PageNumber // Add current page information
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ItemList", itemViewModel);
            }

            return View(itemViewModel);
        }

        public IActionResult Details(int id)
        {
            var item = _itemRepository.GetById(id);
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Add(item);

            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var item = _itemRepository.GetById(id);

            if (item==null)
            {
                return View();
            }

            var itemVM = new CreateItemViewModel { item = item, tagIds = new List<int>() };

            foreach (ItemTag itemTag in item.ItemTags)
            {
                itemVM.tagIds.Add(itemTag.TagId);
            }

            return View(itemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Update(item);

            int id = item.ItemId;
            item = _itemRepository.GetById(id);

            // Delete old tags
            foreach (ItemTag itemTag in item.ItemTags)
            {
                _itemTagRepository.Delete(itemTag);
            }

            // Create new tags
            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            var item = _itemRepository.GetById(id);
            _itemRepository.Delete(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
