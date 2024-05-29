using DAL.Model;

namespace WebShop.Model
{
    public class ArtistListViewModel
    {
        public IEnumerable<Artist> Artists { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
