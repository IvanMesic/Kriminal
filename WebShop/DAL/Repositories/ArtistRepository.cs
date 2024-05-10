using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    public ArtistRepository(DbContext context) : base(context) { }
}