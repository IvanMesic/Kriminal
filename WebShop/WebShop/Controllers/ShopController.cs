using AutoMapper;
using DAL.Interfaces;
using DAL.Model;
using DAL.Repositories;
using DAL.ServiceInterfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Net;
using System.Security.Claims;
using System.Xml.Linq;
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
        private readonly ICartService _cartService;
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;
        private readonly IBidRepository _bidRepository;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionItemRepository _transactionItemRepository;
        private readonly IItemTagRepository _itemTagRepository;

       
        public ShopController(IItemRepository itemRepository, IMapper mapper, ICartService cartService, ITagRepository tagRepository, IBidRepository bidRepository, ITransactionService transactionService, ITransactionRepository transactionRepository, ITransactionItemRepository transactionItemRepository, IArtistRepository artistRepository, ICategoryRepository categoryRepository, IItemTagRepository itemTagRepository)
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
            _itemTagRepository = itemTagRepository;
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

            _cartService.AddItem(item.ItemId, item.Title, item.Price, quantity, item.SaleMultiplier ?? 1m);

            return RedirectToAction(nameof(Index));

        }


        public ActionResult TimeoutBid(int id)
        {


            
            Bid bid = _bidRepository.GetHighestBidForItem(id);

            Item item = _itemRepository.GetById(id);

            if (bid.Amount==null || bid.Amount == 0)
            {
                return Ok();
            }

                item.Sold = true;
                _itemRepository.Update(item);

                var transaction = new Transaction()
                {
                    UserId = bid.UserId,
                    Date = DateTime.Now,
                    TotalAmount = bid.Amount
                };

                _transactionRepository.Add(transaction);

                var transactionItem = new TransactionItem()
                {
                    ItemId = item.ItemId,
                    TransactionId = transaction.TransactionId
                };

                _transactionItemRepository.Add(transactionItem);

                return Ok();
        }

        [HttpPost]
        public JsonResult PlaceBid(int bid, int itemId)
        {
            var highestBid = _bidRepository.GetHighestBidForItem(itemId);
            var item = _itemRepository.GetById(itemId);

            if (bid <= highestBid.Amount || bid <= (int)((double)item.Price * 0.6))
            {
                return Json(new { success = false, message = "Bid too low" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var userId = int.Parse(userIdClaim.Value);

            if (item.OwnerId == userId)
            {
                return Json(new { success = false, message = "Can not bid on your own Auction" });
            }


            Bid tempBid = new Bid()
            {
                UserId = userId,
                ItemId = itemId,
                Amount = bid
            };

            _bidRepository.Add(tempBid);

            var x = item.SaleMultiplier ?? 1;

            if (bid >= item.Price * x)
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
        public ActionResult PrintAllItemsToPdf()
        {
            List<Item> items = _itemRepository.GetAll().ToList();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add a title
                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                Paragraph title = new Paragraph("Items List", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                // Add a table
                PdfPTable table = new PdfPTable(6) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 10f, 20f, 40f, 10f, 10f, 10f });

                // Add table headers
                Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("ID", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Title", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Price", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Created At", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Owner", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Add table rows
                Font rowFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                foreach (var item in items)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.ItemId.ToString(), rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.Title, rowFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(item.Description ?? "", rowFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(item.Price.ToString("C"), rowFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(item.CreatedAt.ToString("dd-MM-yyyy"), rowFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.Owner?.Username ?? "N/A", rowFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                }

                document.Add(table);
                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", "ItemsList.pdf");
            }
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


        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemViewModel itemViewModel)
        {
            var item = _mapper.Map<Item>(itemViewModel.item);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            item.OwnerId = int.Parse(userIdClaim.Value);

            _itemRepository.Add(item);

            foreach (var tagId in itemViewModel.tagIds)
            {
                var itemTag = new ItemTag { ItemId = item.ItemId, TagId = tagId };
                _itemTagRepository.Add(itemTag);
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
