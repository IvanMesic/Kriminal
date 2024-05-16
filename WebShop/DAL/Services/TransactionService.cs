using DAL.Interfaces;
using DAL.Model;
using DAL.ServiceInterfaces;

namespace WebShop.Services
{
    public class TransactionService : ITransactionService
    {
   private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionItemRepository _transactionItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;

        public TransactionService(ITransactionRepository transactionRepository, 
                                  ITransactionItemRepository transactionItemRepository,
                                  IUserRepository userRepository, 
                                  ICartService cartService)
        {
            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _userRepository = userRepository;
            _cartService = cartService;
        }

        public Transaction CreateTransaction(int userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
                throw new Exception("User not found.");

            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                throw new Exception("Cart is empty.");

            var transaction = new Transaction
            {
                UserId = userId,
                Date = DateTime.Now,
                TotalAmount = cart.Total,
                TransactionItems = cart.Items.Select(i => new TransactionItem
                {
                    ItemId = i.ItemId
                }).ToList()
            };

            _transactionRepository.Add(transaction);
            /*
            foreach (var transactionItem in transaction.TransactionItems)
            {

                transactionItem.TransactionId = transaction.TransactionId;
                _transactionItemRepository.Add(transactionItem);
            }
            */
            _cartService.ClearCart();
            return transaction;
        }

        public IQueryable<Transaction> GetUserTransactions(int userId)
        {
            throw new NotImplementedException();
        }


        public  Transaction GetTransactionById(int id)
        {
            return _transactionRepository.GetById(id);
        }
    }
}

