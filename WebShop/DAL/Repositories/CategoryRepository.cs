using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DbContext context) : base(context) { }
}