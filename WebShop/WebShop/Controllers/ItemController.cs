using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
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
        private readonly IBidRepository _bidRepository;

        private readonly IMapper _mapper;

        public ItemController(
            IItemRepository itemRepository,
            ITagRepository tagRepository,
            IItemTagRepository itemTagRepository,
            IMapper mapper, 
            ICategoryRepository categoryRepository,
            IArtistRepository artistRepository,
            IBidRepository bidRepository)
        {
            _itemRepository = itemRepository;

            _tagRepository = tagRepository;
            _itemTagRepository = itemTagRepository;
            _categoryRepository = categoryRepository;
            _artistRepository = artistRepository;

            _mapper = mapper;
            _bidRepository = bidRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "No file selected." });

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/static-files", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = "/static-files/" + file.FileName;
            return Json(new { success = true, filePath = relativePath });
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(ItemListViewModel itemListViewModel)
        {
            int pageSize = 10;
            int pageNumber = (itemListViewModel.pageNumber ?? 1);

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
            itemListViewModel.pageNumber = pageNumber;//SET THESE

            var filteredItemsPaged = filteredItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            itemListViewModel.items = filteredItemsPaged.ToArray();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsListPartial", itemListViewModel);
            }

            return View(itemListViewModel);
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult Details(int id)
        {
            var item = _itemRepository.GetById(id);

            if(item != null)
            {
                return View(item);
            }

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create(CreateItemViewModel itemViewModel)
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

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
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
        [Authorize(Roles = "Admin, User")]
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

        [Authorize(Roles = "Admin, User")]
        public ActionResult GetItemsForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Item> items = _itemRepository.GetAllItemsForUser(int.Parse(userId)).ToList();

            return View(items);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult GetUserBids()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bids = _bidRepository.GetHighestBidsForUser(int.Parse(userId)) ?? new List<Bid>();

            return View(bids);
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

