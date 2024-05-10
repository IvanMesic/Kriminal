using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class TransactionItemRepository : Repository<TransactionItem>, ITransactionItemRepository
{
    public TransactionItemRepository(DbContext context) : base(context) { }
}