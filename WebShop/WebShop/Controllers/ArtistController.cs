using DAL.Interfaces;
using DAL.Model;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class ArtistController : Controller
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistController(IArtistRepository artistRepository)
        {
            this._artistRepository = artistRepository;
        }

        // GET: ArtistController
        public ActionResult Index()
        {
            var artists = _artistRepository.GetAll();
            return View(artists);
        }

        // GET: ArtistController/Details/5
        public ActionResult Details(int id)
        {
            var artist = _artistRepository.GetById(id);
            return View(artist);
        }

        // GET: ArtistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist artist)
        {
            try
            {
                _artistRepository.Add(artist);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArtistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArtistController/Delete/5
        public ActionResult Delete(int id)
        {
            var artist = _artistRepository.GetById(id);
            _artistRepository.Delete(artist);
            return RedirectToAction(nameof(Index));
        }

    }
}
