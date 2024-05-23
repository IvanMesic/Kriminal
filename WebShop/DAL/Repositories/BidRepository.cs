using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
                .Include(b => b.User)
            .ToList();
    }
    public override Bid GetById(int id)
    {
        return _context.Bid
            .Include(b => b.User)
            .FirstOrDefault(b => b.BidId == id);
    }

    public IList<Bid> GetAllBidsForItem(int itemId)
    {
        return GetAll().Where(b => b.ItemId == itemId).OrderBy(b => b.Amount).Reverse().ToList();
    }

    public Bid GetHighestBidForItem(int itemId)
    {
        var bids = GetAll().Where(b => b.ItemId == itemId).OrderBy(b => b.Amount).Reverse();
        if (bids.IsNullOrEmpty())
        {
            return new Bid()
            {
                ItemId = itemId,
                Amount = 0
            };
        }

        return GetAll().Where(b => b.ItemId == itemId).OrderBy(b => b.Amount).Reverse().First();
    }

    public IEnumerable<Bid> GetHighestBidsForUser(int userId)
    {
        var highestBids = _context.Bid
            .Include(u => u.User)
            .Include(u => u.Item)
            .Where(b => b.UserId == userId)
            .GroupBy(b => b.ItemId)
            .Select(g => g.OrderByDescending(b => b.Amount).First())
            .ToList();

        return highestBids;
    }

}