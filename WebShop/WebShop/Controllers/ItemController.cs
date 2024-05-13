using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebShop.Models.ViewModel;
using WebShop.Services;

namespace WebShop.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IItemRepository _itemRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly IMapper _mapper;


        public ItemController(IItemTagRepository itemTagRepository, IItemRepository itemRepository, IItemService itemService, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _itemService = itemService;
            _itemTagRepository = itemTagRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var items = _itemRepository.GetAll();
            return View(items);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Add(item);


            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }

            return RedirectToAction(nameof(Index));
        }



        public ActionResult Edit(int id)
        {
            var viewModel = _itemService.GetItemById(id);
            if (viewModel == null)
            {
                return NotFound();
            }


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _itemService.UpdateItem(viewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        public ActionResult Delete(int id)
        {
            var viewModel = _itemService.GetItemById(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _itemService.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }
    }
 }
