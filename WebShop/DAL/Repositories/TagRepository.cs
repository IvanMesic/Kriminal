using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly DataContext _context;
        public TagRepository(DataContext context) : base(context) {
        
            _context = context;
        }

        public Tag GetByName(string name)
        {
            return _context.Tag.FirstOrDefault(t => t.Name == name);
        }
    }
}
