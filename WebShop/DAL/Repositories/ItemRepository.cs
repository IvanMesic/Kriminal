using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(DbContext context) : base(context) { }
}