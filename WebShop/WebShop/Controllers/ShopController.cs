using AutoMapper;
using DAL.Interfaces;
using DAL.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        ICartService _cartService;
        IItemRepository _itemRepository;
        IMapper _mapper;

        public ShopController(IItemRepository itemRepository, IMapper mapper, ICartService cartService)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _cartService = cartService;
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

        public ActionResult Index()
        {
            var items = _itemRepository.GetAll();
            return View(items);
        }

    }
}
