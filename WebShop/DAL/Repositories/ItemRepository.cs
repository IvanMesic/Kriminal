using DAL.Data;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

public class ItemRepository : Repository<Item>, IItemRepository
{
    private readonly DataContext _context;

    public ItemRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override IList<Item> GetAll()
    {
        return _context.Item
            .Include(i => i.Artist)
            .Include(i => i.Category)
            .Include(i => i.ItemTags)
                .ThenInclude(it => it.Tag)
            .Include(i => i.Owner)
            .ToList();
    }

    public IList<Item> GetAllApi()
    {
        return _context.Item.ToList();
    }

    public override Item GetById(int id)
    {
        return _context.Item
            .Include(i => i.Artist)
            .Include(i => i.Category)
            .Include(i => i.ItemTags)
                .ThenInclude(it => it.Tag)
                .Include(i => i.Owner)
            .FirstOrDefault(i => i.ItemId == id);
    }

    public IList<Item> GetFiltered(
        IList<Tag>? tags = null,
        IList<Artist>? artists = null,
        IList<Category>? categories = null,
        decimal? priceMin = 0,
        decimal? priceMax = null,
        string? searchQuery = null,
        bool includeSold = false,
        bool includeSale = false,
        string? sortBy = null,
        string? sortOrder = "asc")
    {
        var query = _context.Item.AsQueryable();

        // Collect artist IDs based on specified tags
        var artistIdsWithTag = new List<int>();
        if (tags != null && tags.Any())
        {
            var tagIds = tags.Select(t => t.TagId).ToList();
            artistIdsWithTag = _context.Artist
                .Where(a => a.ArtistTags.Any(at => tagIds.Contains(at.TagId)))
                .Select(a => a.ArtistId)
                .ToList();
        }

        if (tags != null && tags.Any())
        {
            foreach (var tag in tags)
            {
                query = query.Where(i => i.ItemTags.Any(it => it.TagId == tag.TagId) || artistIdsWithTag.Contains(i.ArtistId));
            }
        }

        if (artists != null && artists.Any())
        {
            var artistIds = artists.Select(a => a.ArtistId);
            query = query.Where(i => artistIds.Contains(i.ArtistId));
        }

        if (categories != null && categories.Any())
        {
            var categoryIds = categories.Select(c => c.CategoryId);
            query = query.Where(i => categoryIds.Contains(i.CategoryId));
        }

        if (priceMin != null)
        {
            query = query.Where(i => i.Price >= priceMin);
        }

        if (priceMax != null)
        {
            query = query.Where(i => i.Price <= priceMax);
        }

        if (!includeSold)
        {
            query = query.Where(i => !i.Sold);
        }

        if (includeSale)
        {
            query = query.Where(i => i.SaleMultiplier < 1);
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(i => i.Title.Contains(searchQuery) || i.Description.Contains(searchQuery));
        }

        var result = query.Include(i => i.Artist)
                          .Include(i => i.Category)
                          .Include(i => i.ItemTags)
                              .ThenInclude(it => it.Tag)
                          .Include(i => i.Owner)
                          .Distinct()
                          .ToList();

        // Apply sorting after fetching the results
        switch (sortBy)
        {
            case "price":
                result = sortOrder == "desc" ? result.OrderByDescending(i => i.Price).ToList() : result.OrderBy(i => i.Price).ToList();
                break;
            case "name":
                result = sortOrder == "desc" ? result.OrderByDescending(i => i.Title).ToList() : result.OrderBy(i => i.Title).ToList();
                break;
            default:
                result = sortOrder == "desc" ? result.OrderByDescending(i => i.ItemId).ToList() : result.OrderBy(i => i.ItemId).ToList();
                break;
        }

        return result;
    }

    public IEnumerable<Item> GetAllItemsForUser(int userId)
    {
        return _context.Item.Where(i => i.OwnerId == userId);
    }
}
