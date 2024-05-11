using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebShop.Models.ViewModel;

namespace WebShop.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository itemRepository;
        private readonly IMapper _mapper;

        public ItemController(IItemRepository itemRepository, IMapper mapper)
        {
            _mapper = mapper;
            this.itemRepository = itemRepository;
        }

        // Display a list of all items
        public IActionResult Index()
        {
            var items = itemRepository.GetAll();
            var viewModels = _mapper.Map<List<ItemViewModel>>(items);
            return View(viewModels);
        }

        // Display details for a specific item
        public ActionResult Details(int id)
        {
            Item item = itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<ItemViewModel>(item);
            return View(viewModel);
        }

        // Display the item creation form
        public ActionResult Create()
        {
            return View();
        }

        // Handle item creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Item item = _mapper.Map<Item>(viewModel);
                    itemRepository.Add(item);
                    return RedirectToAction(nameof(Index));
                }
                return View(viewModel);
            }
            catch
            {
                return View(viewModel);
            }
        }

        public ActionResult Edit(int id)
        {
            Item item = itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<ItemViewModel>(item);
            return View(viewModel);
        }

        // Process item updates
        [HttpPost]
        public ActionResult Save(ItemViewModel viewModel)
        {


            if (ModelState.IsValid)
            {
                Item item = _mapper.Map<Item>(viewModel);
                try
                {
                    itemRepository.Update(item);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(viewModel);
        }

        // Display the delete confirmation page
        public ActionResult Delete(int id)
        {
            Item item = itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<ItemViewModel>(item);
            return View(viewModel);
        }

        // Handle item deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            itemRepository.Delete(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
