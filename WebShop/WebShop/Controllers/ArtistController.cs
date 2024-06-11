using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Model;
using WebShop.Services.Interfaces;

namespace WebShop.Controllers
{
    public class ArtistController : Controller
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var viewModel = await _artistService.GetArtistsAsync(searchString, pageNumber, pageSize);
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var artist = _artistService.GetArtistById(id);
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
            _artistService.CreateArtist(artistViewModel);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int id)
        {
            var artist = _artistService.GetArtistById(id);

            if (artist == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var artistVM = new CreateArtistViewModel { artist = artist, tagIds = new List<int>() };

            foreach (ArtistTag artistTag in artist.ArtistTags)
            {
                artistVM.tagIds.Add(artistTag.TagId);
            }

            return View(artistVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(CreateArtistViewModel artistViewModel)
        {
            _artistService.UpdateArtist(artistViewModel);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int id)
        {
            _artistService.DeleteArtist(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
