using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemTagRepository : Repository<ItemTag>, IItemTagRepository
{
    public ItemTagRepository(DataContext context) : base(context) { }
}