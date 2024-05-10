using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ActionItemRepository : Repository<ActionItem>, IActionItemRepository
{
    public ActionItemRepository(DataContext context) : base(context) { }
}