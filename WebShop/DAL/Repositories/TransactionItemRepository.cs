using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class TransactionItemRepository : Repository<TransactionItem>, ITransactionItemRepository
{
    public TransactionItemRepository(DataContext context) : base(context) { }
}