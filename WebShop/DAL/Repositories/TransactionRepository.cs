using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DataContext context) : base(context) { }
}