﻿using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using DAL.Repositories;
using DAL.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebShop.Model;

namespace WebShop.Controllers
{
    public class ShopController : Controller
    {

        /// <summary>
        ///todo
        ///Add Images to Views and CartItem
        /// /Make View Look Nice 
        /// Add direct Purchase option when Transactions Are implemented
        /// </summary>

        private readonly ITagRepository _tagRepository;
        ICartService _cartService;
        IItemRepository _itemRepository;
        ICategoryRepository _categoryRepository;
        IArtistRepository _artistRepository;
        IMapper _mapper;
        private readonly IBidRepository _bidRepository;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionItemRepository _transactionItemRepository;
        

       
        public ShopController(IItemRepository itemRepository, IMapper mapper, ICartService cartService, ITagRepository tagRepository, IBidRepository bidRepository, ITransactionService transactionService, ITransactionRepository transactionRepository, ITransactionItemRepository transactionItemRepository, IArtistRepository artistRepository, ICategoryRepository categoryRepository)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _cartService = cartService;
            _tagRepository = tagRepository;
            _bidRepository = bidRepository;
            _transactionService = transactionService;
            _transactionRepository = transactionRepository;
            _transactionItemRepository = transactionItemRepository;
            _artistRepository = artistRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var item = _itemRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            _cartService.AddItem(item.ItemId, item.Title, item.Price, quantity);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public JsonResult PlaceBid(int bid, int itemId)
        {
            var highestBid = _bidRepository.GetHighestBidForItem(itemId);
            var item = _itemRepository.GetById(itemId);

            if (bid <= highestBid.Amount)
            {
                return Json(new { success = false, message = "Bid too low" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var userId = int.Parse(userIdClaim.Value);
            Bid tempBid = new Bid()
            {
                UserId = userId,
                ItemId = itemId,
                Amount = bid
            };

            _bidRepository.Add(tempBid);

            if (bid >= item.Price)
            {
                item.Sold = true;
                _itemRepository.Update(item);

                var transaction = new Transaction()
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    TotalAmount = bid
                };

                _transactionRepository.Add(transaction);

                var transactionItem = new TransactionItem()
                {
                    ItemId = item.ItemId,
                    TransactionId = transaction.TransactionId
                };

                _transactionItemRepository.Add(transactionItem);

                return Json(new { success = true, redirectUrl = Url.Action("TransactionDetails", "Transaction", new { id = transaction.TransactionId }) });
            }

            return Json(new { success = true });
        }


        public ActionResult GetBids(int itemId)
        {
            var bids = _bidRepository.GetAllBidsForItem(itemId);
            return PartialView("_BidsList", bids);
        }


        [HttpGet]
        public ActionResult ItemDetails(int id)
        {
            var item = _itemRepository.GetById(id);

            if(item != null)
            {
                var Bids = _bidRepository.GetAllBidsForItem(id);

                var highestBid = _bidRepository.GetHighestBidForItem(id);
                
                ViewBag.Bids = Bids;
                ViewBag.HighestBid = highestBid;


                return View(item);
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Index(ItemListViewModel itemListViewModel)
        {
            int pageSize = 10;
            int pageNumber = (itemListViewModel.pageNumber ?? 1);

            foreach (int tagId in itemListViewModel.selectedTags)
            {
                itemListViewModel.tags.Add(_tagRepository.GetById(tagId));
            }

            foreach (int categoryId in itemListViewModel.selectedCategories)
            {
                itemListViewModel.categories.Add(_categoryRepository.GetById(categoryId));
            }

            foreach (int artistId in itemListViewModel.selectedArtists)
            {
                itemListViewModel.artists.Add(_artistRepository.GetById(artistId));
            }

            IList<Item> filteredItems = _itemRepository.GetFiltered(
                tags: itemListViewModel.tags,
                artists: itemListViewModel.artists,
                categories: itemListViewModel.categories,
                priceMin: itemListViewModel.priceMin,
                priceMax: itemListViewModel.priceMax,
                searchQuery: itemListViewModel.searchQuery
                );

            int totalPages = (int)Math.Ceiling((double)filteredItems.Count() / pageSize);

            ViewBag.TotalPages = totalPages; //SET THESE
            itemListViewModel.pageNumber = pageNumber;//SET THESE

            var filteredItemsPaged = filteredItems.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            itemListViewModel.items = filteredItemsPaged.ToArray();


            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsListPartialShop", itemListViewModel);
            }

            return View(itemListViewModel);
        }
    }
}
