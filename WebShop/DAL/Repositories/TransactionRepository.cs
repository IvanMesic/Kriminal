using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DbContext context) : base(context) { }
}