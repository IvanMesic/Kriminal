using AutoMapper;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Model;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var relativePath = await _itemService.UploadImageAsync(file);
                return Json(new { success = true, filePath = relativePath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(ItemListViewModel itemListViewModel)
        {
            var (filteredItems, totalPages) = _itemService.GetFilteredItems(itemListViewModel);

            ViewBag.TotalPages = totalPages; // SET THESE
            itemListViewModel.pageNumber = itemListViewModel.pageNumber ?? 1; // SET THESE

            var filteredItemsPaged = filteredItems.Skip((itemListViewModel.pageNumber.Value - 1) * 10).Take(10);

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
            var item = _itemService.GetItemById(id);

            if (item != null)
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
            _itemService.CreateItem(itemViewModel);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id)
        {
            var itemVM = _itemService.GetEditItemViewModel(id);

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
            _itemService.EditItem(itemViewModel);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int id)
        {
            _itemService.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult GetItemsForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _itemService.GetAllItemsForUser(int.Parse(userId)).ToList();
            return View(items);
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult GetUserBids()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bids = _itemService.GetHighestBidsForUser(int.Parse(userId)) ?? new List<Bid>();
            return View(bids);
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
