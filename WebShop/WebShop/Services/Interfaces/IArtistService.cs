using DAL.Model;
using WebShop.Model;

namespace WebShop.Services.Interfaces
{
    public interface IArtistService
    {
        Task<ArtistListViewModel> GetArtistsAsync(string searchString, int pageNumber, int pageSize);
        Artist GetArtistById(int id);
        void CreateArtist(CreateArtistViewModel artistViewModel);
        void UpdateArtist(CreateArtistViewModel artistViewModel);
        void DeleteArtist(int id);
    }
}
