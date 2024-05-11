using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemRepository : Repository<Item>, IItemRepository
{

    private readonly DataContext _context;

    public ItemRepository(DataContext context) : base(context) 
    { 
        _context = context;
    }


    public override IList<Item> GetAll()
    {
        return _context.Item
            .Include(i => i.Artist)
            .Include(i => i.Category)
            .Include(i => i.ItemTags)
                .ThenInclude(it => it.Tag)
            .ToList();
    }
    public override Item GetById(int id)
    {
        return _context.Item
            .Include(i => i.Artist)
            .Include(i => i.Category)
            .Include(i => i.ItemTags)
                .ThenInclude(it => it.Tag)
            .FirstOrDefault(i => i.ItemId == id);
    }






}