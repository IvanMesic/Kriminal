using DAL.Model;

namespace WebShop.Model
{
    public class CreateArtistViewModel
    {
        public Artist artist { get; set; }
        public List<int> tagIds { get; set; }
    }
}
