using DAL.Interfaces;
using DAL.Model;
using DAL.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Services;

namespace WebShop.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionService transactionService, ITransactionRepository transactionRepository)
        {
            _transactionService = transactionService;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult GetTransactionsForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Transaction> transactions = _transactionRepository.GetTransactionsForUser(int.Parse(userId)).ToList();

            return View(transactions);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTransaction()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var transaction = _transactionService.CreateTransaction(int.Parse(userId));
                return RedirectToAction(nameof(TransactionDetails), new { id = transaction.TransactionId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult UserTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User is not authenticated");
            }

            var transactions = _transactionService.GetUserTransactions(int.Parse(userId));
            return View(transactions);
        }

        [HttpGet]
        public  IActionResult TransactionDetails(int id)
        {
            var transaction =  _transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
    }
}

