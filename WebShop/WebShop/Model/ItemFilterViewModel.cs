using DAL.Model;

namespace WebShop.Model
{
    public class ItemListViewModel
    {
        public IList<Tag> tags { get; set; } = new List<Tag>();
        public IList<Artist> artists { get; set; } = new List<Artist>();
        public IList<Category> categories { get; set; } = new List<Category>();

        public IList<int> selectedTags { get; set; } = new List<int>();
        public IList<int> selectedArtists {  get; set; } = new List<int>();
        public IList<int> selectedCategories { get; set; } = new List<int>();

        public decimal? priceMin { get; set; }
        public decimal? priceMax { get; set; }
        public string? searchQuery { get; set; }

        public IList<Item>? items { get; set; }

    }
}
