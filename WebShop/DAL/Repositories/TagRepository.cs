namespace DAL.Repositories
{
    using DAL.Interfaces;
    using DAL.Model;
    using Microsoft.EntityFrameworkCore;

    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DbContext context) : base(context) { }
    }
}
