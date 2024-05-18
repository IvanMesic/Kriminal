using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [Route("api/itemList")]
    [ApiController]
    public class ItemListController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemListController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet("GetAllItems")]
        public IActionResult GetAllItems()
        {
            var items = _itemRepository.GetAllApi();
            if (items == null || !items.Any())
            {
                return NotFound("No items found.");
            }

            return Ok(items);
        }
    }
}
