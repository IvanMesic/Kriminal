using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class BidRepository : Repository<Bid>, IBidRepository
{

    public BidRepository(DataContext context) : base(context) 
    {
        
    }
}