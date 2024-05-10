using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    public ArtistRepository(DataContext context) : base(context) { }
}