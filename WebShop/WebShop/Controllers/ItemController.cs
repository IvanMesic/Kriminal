﻿using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ActionResult Index(ItemFilterViewModel filterModel)
        {
            // Ensure PageNumber and PageSize have valid values
            filterModel.PageNumber = filterModel.PageNumber < 1 ? 1 : filterModel.PageNumber;
            filterModel.PageSize = filterModel.PageSize < 1 ? 10 : filterModel.PageSize;

            var filteredItems = _itemFilterService.GetFilteredItems(filterModel);
            int totalPages = _itemFilterService.getTotalPages();

            var filterOptions = _itemFilterService.GetFilterOptions();
            filterModel.Categories = filterOptions.Categories;
            filterModel.Artists = filterOptions.Artists;
            filterModel.Tags = filterOptions.Tags;

            var itemViewModel = new ItemListViewModel
            {
                Items = filteredItems,
                Filter = filterModel,
                TotalPages = totalPages,
                CurrentPage = filterModel.PageNumber
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsListPartial", itemViewModel);
            }

            return View(itemViewModel);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            var item = _itemRepository.GetById(id);
            return View(item);
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: HomeController1/Edit/5

        public ActionResult Edit(int id)
        {
            var item = _itemRepository.GetById(id);

            if (item == null)
            {
                return View();
            }

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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }

    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }
    }
}
