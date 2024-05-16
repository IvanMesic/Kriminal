using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using WebShop.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebShop.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        
        private readonly ITagRepository _tagRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArtistRepository _artistRepository;

        private readonly IMapper _mapper;

        public ItemController(
            IItemRepository itemRepository,
            ITagRepository tagRepository,
            IItemTagRepository itemTagRepository,
            IMapper mapper, 
            ICategoryRepository categoryRepository, 
            IArtistRepository artistRepository)
        {
            _itemRepository = itemRepository;

            _tagRepository = tagRepository;
            _itemTagRepository = itemTagRepository;
            _categoryRepository = categoryRepository;
            _artistRepository = artistRepository;

            _mapper = mapper;
        }

        public ActionResult Index(ItemListViewModel itemListViewModel, int? pageNum)
        {
            int pageSize = 10;
            int pageNumber = (pageNum ?? 1);

            foreach (int tagId in itemListViewModel.selectedTags)
            {
                itemListViewModel.tags.Add(_tagRepository.GetById(tagId));
            }

            foreach(int categoryId in itemListViewModel.selectedCategories)
            {
                itemListViewModel.categories.Add(_categoryRepository.GetById(categoryId));
            }

            foreach(int artistId in itemListViewModel.selectedArtists)
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
            ViewBag.CurrentPage = pageNumber;//SET THESE

            var filteredItemsPaged = filteredItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            itemListViewModel.items = filteredItemsPaged.ToArray();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsListPartial", itemListViewModel);
            }

            return View(itemListViewModel);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            var item = _itemRepository.GetById(id);

            if(item != null)
            {
                return View(item);
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5

        public ActionResult Edit(int id)
        {
            var item = _itemRepository.GetById(id);

            if (item == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var itemVM = new CreateItemViewModel { item = item, tagIds = new List<int>() };

            foreach (ItemTag itemTag in item.ItemTags)
            {
                itemVM.tagIds.Add(itemTag.TagId);
            }

            if (itemVM != null)
            {
                return View(itemVM);
            }

            return RedirectToAction(nameof(Index));
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }

    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }
    }
}
