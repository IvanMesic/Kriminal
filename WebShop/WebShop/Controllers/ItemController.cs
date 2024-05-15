using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using DAL.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebShop.Model;
using WebShop.Services;

namespace WebShop.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemFilterService _itemFilterService;
        private readonly IItemRepository _itemRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly IMapper _mapper;

        public ItemController(IItemFilterService itemFilterService, IItemRepository itemRepository,
                              IItemTagRepository itemTagRepository, IMapper mapper)
        {
            _itemFilterService = itemFilterService;
            _itemRepository = itemRepository;
            _itemTagRepository = itemTagRepository;
            _mapper = mapper;
        }

        public IActionResult Index(ItemFilterViewModel filterModel)
        {
            var items = _itemFilterService.GetFilteredItems(filterModel);
            var filterOptions = _itemFilterService.GetFilterOptions();

            filterModel.Categories = filterOptions.Categories;
            filterModel.Artists = filterOptions.Artists;
            filterModel.Tags = filterOptions.Tags;

            var itemViewModel = new ItemListViewModel
            {
                Items = items,
                Filter = filterModel
            };

            return View(itemViewModel);
        }

        public IActionResult Details(int id)
        {
            var item = _itemRepository.GetById(id);
            return View(item);
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
            var item = _itemRepository.GetById(id);
            var itemVM = new CreateItemViewModel { item = item, tagIds = new List<int>() };

            foreach (ItemTag itemTag in item.ItemTags)
            {
                itemVM.tagIds.Add(itemTag.TagId);
            }

            return View(itemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);
            _itemRepository.Update(item);

            int id = item.ItemId;
            item = _itemRepository.GetById(id);

            // Delete old tags
            foreach (ItemTag itemTag in item.ItemTags)
            {
                _itemTagRepository.Delete(itemTag);
            }

            // Create new tags
            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            var item = _itemRepository.GetById(id);
            _itemRepository.Delete(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
