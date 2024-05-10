using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ActionItemRepository : Repository<ActionItem>, IActionItemRepository
{
    public ActionItemRepository(DbContext context) : base(context) { }
}