using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly DataContext _context;

    public TransactionRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override Transaction GetById(int id)
    {
        return _context.Transaction
            .Include(a => a.TransactionItems)
                    .ThenInclude(ti => ti.Item)
            .Include(a => a.User).FirstOrDefault(a => a.TransactionId == id);
    }
}

