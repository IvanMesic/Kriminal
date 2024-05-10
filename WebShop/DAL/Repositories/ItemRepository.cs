using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(DataContext context) : base(context) { }
}