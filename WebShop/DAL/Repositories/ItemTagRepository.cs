using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemTagRepository : Repository<ItemTag>, IItemTagRepository
{
    public ItemTagRepository(DbContext context) : base(context) { }
}