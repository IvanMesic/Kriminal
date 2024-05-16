using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using DAL.Repositories;
using DAL.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Model;

namespace WebShop.Controllers
{
    public class ShopController : Controller
    {

        /// <summary>
        ///todo
        ///Add Images to Views and CartItem
        /// /Make View Look Nice 
        /// Add direct Purchase option when Transactions Are implemented
        /// </summary>

        private readonly ITagRepository _tagRepository;
        ICartService _cartService;
        IItemRepository _itemRepository;
        ICategoryRepository _categoryRepository;
        IArtistRepository _artistRepository;
        IMapper _mapper;

        public ShopController(IItemRepository itemRepository, IMapper mapper, ICartService cartService, ITagRepository tagRepository)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _cartService = cartService;
            _tagRepository = tagRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var item = _itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            _cartService.AddItem(item.ItemId, item.Title, item.Price, quantity);
            return RedirectToAction(nameof(Index));

        }

        public ActionResult Index(ItemListViewModel itemListViewModel)
        {
            int pageSize = 10;
            int pageNumber = (itemListViewModel.pageNumber ?? 1);

            foreach (int tagId in itemListViewModel.selectedTags)
            {
                itemListViewModel.tags.Add(_tagRepository.GetById(tagId));
            }

            foreach (int categoryId in itemListViewModel.selectedCategories)
            {
                itemListViewModel.categories.Add(_categoryRepository.GetById(categoryId));
            }

            foreach (int artistId in itemListViewModel.selectedArtists)
            {
                itemListViewModel.artists.Add(_artistRepository.GetById(artistId));
            }

            IList<Item> filteredItems = _itemRepository.GetFiltered(
                tags: itemListViewModel.tags,
                artists: itemListViewModel.artists,
                categories: itemListViewModel.categories,
                priceMin: itemListViewModel.priceMin,
                priceMax: itemListViewModel.priceMax,
                searchQuery: itemListViewModel.searchQuery
                );

            int totalPages = (int)Math.Ceiling((double)filteredItems.Count() / pageSize);

            ViewBag.TotalPages = totalPages; //SET THESE
            itemListViewModel.pageNumber = pageNumber;//SET THESE

            var filteredItemsPaged = filteredItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            itemListViewModel.items = filteredItemsPaged.ToArray();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsListPartialShop", itemListViewModel);
            }

            return View(itemListViewModel);
        }
    }
}
