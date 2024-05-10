using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class UserBidRepository : Repository<UserBid>, IUserBidRepository
{
    public UserBidRepository(DbContext context) : base(context) { }
}