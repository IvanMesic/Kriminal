using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    private readonly DataContext _context;

    public ArtistRepository(DataContext context) : base(context)
    {
        _context = context;
    }
    public override IList<Artist> GetAll()
    {
        return _context.Artist
            .Include(a => a.Items)
            .Include(a => a.ArtistTags)
                .ThenInclude(a=> a.Tag)
            .ToList();
    }
    public override Artist GetById(int id)
    {
        return _context.Artist
            .Include(a => a.Items)
            .Include(a => a.ArtistTags)
                .ThenInclude(a => a.Tag)
            .FirstOrDefault(a => a.ArtistId == id);
    }

    public List<Item> GetWorksOfArtist(int ArtistId)
    {
        var artist = GetById(ArtistId);
        return artist.Items.ToList();
    }
}