using DAL.Model;
using DAL.ServiceInterfaces;
using DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using WebShop.Services;

namespace WebShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ITransactionService _transactionService;

        public CartController(ICartService cartService, ITransactionService transactionService)
        {
            _cartService = cartService;
            _transactionService = transactionService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int itemId)
        {
            _cartService.RemoveItem(itemId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CommitTransaction()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User is not authenticated");
            }

            int userId = int.Parse(userIdClaim.Value);

            try
            {
                var transaction = _transactionService.CreateTransaction(userId);
                return RedirectToAction("TransactionDetails", "Transaction", new { id = transaction.TransactionId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var cart = _cartService.GetCart();
                return View("Index", cart);
            }
        }
    }
}

