using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class UserBidRepository : Repository<UserBid>, IUserBidRepository
{
    public UserBidRepository(DataContext context) : base(context) { }
}