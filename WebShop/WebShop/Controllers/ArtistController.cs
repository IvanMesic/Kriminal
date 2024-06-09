using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Model;

namespace WebShop.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IArtistTagRepository _artistTagRepository;
        private readonly IArtistRepository _artistRepository;

        private readonly IMapper _mapper;

        public ArtistController(
            IItemRepository itemRepository,
            ITagRepository tagRepository,
            IArtistTagRepository artistTagRepository,
            IMapper mapper,
            IArtistRepository artistRepository)
        {

            _tagRepository = tagRepository;
            _artistTagRepository = artistTagRepository;
            _artistRepository = artistRepository;

            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var artists = _artistRepository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
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

            return View(viewModel);
        }

        // GET: ArtistController/Details/5
        public ActionResult Details(int id)
        {
            var artist = _artistRepository.GetById(id);
            return View(artist);
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateArtistViewModel artistViewModel)
        {
            var artist = _mapper.Map<Artist>(artistViewModel.artist);
            _artistRepository.Add(artist);

            foreach (var tagId in artistViewModel.tagIds)
            {
                var artistTag = new ArtistTag { ArtistId = artist.ArtistId, TagId = tagId };
                _artistTagRepository.Add(artistTag);
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id)
        {
            var artist = _artistRepository.GetById(id);

            if (artist == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var artistVM = new CreateArtistViewModel { artist = artist, tagIds = new List<int>() };

            foreach (ArtistTag artistTag in artist.ArtistTags)
            {
                artistVM.tagIds.Add(artistTag.TagId);
            }

            if (artistVM != null)
            {
                return View(artistVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(CreateArtistViewModel artistViewModel)
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

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin, User")]
        // GET: ArtistController/Delete/5
        public ActionResult Delete(int id)
        {
            var artist = _artistRepository.GetById(id);
            _artistRepository.Delete(artist);
            return RedirectToAction(nameof(Index));
        }

    }
}
