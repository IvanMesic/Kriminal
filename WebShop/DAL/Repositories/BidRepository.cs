using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class BidRepository : Repository<Bid>, IBidRepository
{

    private readonly DataContext _context;

    public BidRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override IList<Bid> GetAll()
    {
        return _context.Bid
            .Include(b => b.UserBids)
                .ThenInclude(ub => ub.User)
            .ToList();
    }
    public override Bid GetById(int id)
    {
        return _context.Bid
            .Include(b => b.UserBids)
                .ThenInclude(ub => ub.User)
            .FirstOrDefault(b => b.BidId == id);
    }

    public IList<Bid> GetAllBidsForItem(int itemId)
    {
        return GetAll().Where(b => b.ItemId == itemId).OrderBy(b => b.Amount).ToList();
    }

    public Bid GetHighestBidForItem(int itemId)
    {
        return GetAll().Where(b => b.ItemId == itemId).OrderBy(b => b.Amount).First();
    }
}