using DAL.Model;

namespace DAL.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetTransactionsForUser(int id);
    }

}
