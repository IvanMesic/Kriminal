using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(DataContext context) : base(context) { }
}