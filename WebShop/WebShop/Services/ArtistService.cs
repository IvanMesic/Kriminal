using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using WebShop.Model;
using WebShop.Services.Interfaces;

namespace WebShop.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IArtistTagRepository _artistTagRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(
            ITagRepository tagRepository,
            IArtistTagRepository artistTagRepository,
            IArtistRepository artistRepository,
            IMapper mapper)
        {
            _tagRepository = tagRepository;
            _artistTagRepository = artistTagRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<ArtistListViewModel> GetArtistsAsync(string searchString, int pageNumber, int pageSize)
        {


            var artists = _artistRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(a => a.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalArtists = artists.Count();
            var totalPages = (int)Math.Ceiling(totalArtists / (double)pageSize);

            artists = artists.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new ArtistListViewModel
            {
                Artists = artists.ToList(),
                SearchString = searchString,
                PageNumber = pageNumber,
                TotalPages = totalPages
            };

            return viewModel;
        }

        public Artist GetArtistById(int id)
        {
            return _artistRepository.GetById(id);
        }

        public void CreateArtist(CreateArtistViewModel artistViewModel)
        {
            var artist = _mapper.Map<Artist>(artistViewModel.artist);
            _artistRepository.Add(artist);

            foreach (var tagId in artistViewModel.tagIds)
            {
                var artistTag = new ArtistTag { ArtistId = artist.ArtistId, TagId = tagId };
                _artistTagRepository.Add(artistTag);
            }
        }

        public void UpdateArtist(CreateArtistViewModel artistViewModel)
        {
            var artist = artistViewModel.artist;
            _artistRepository.Update(artist);

            int id = artist.ArtistId;
            artist = _artistRepository.GetById(id);

            // Delete old tags
            foreach (ArtistTag artistTag in artist.ArtistTags)
            {
                _artistTagRepository.Delete(artistTag);
            }

            // Create new tags
            foreach (var tagId in artistViewModel.tagIds)
            {
                var artistTag = new ArtistTag { ArtistId = artist.ArtistId, TagId = tagId };
                _artistTagRepository.Add(artistTag);
            }
        }

        public void DeleteArtist(int id)
        {
            var artist = _artistRepository.GetById(id);
            _artistRepository.Delete(artist);
        }
    }
}
