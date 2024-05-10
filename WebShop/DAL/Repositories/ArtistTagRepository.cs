using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ArtistTagRepository : Repository<ArtistTag>, IArtistTagRepository
{
    public ArtistTagRepository(DbContext context) : base(context) { }
}