using DAL.Model;

namespace WebShop.Model
{
    public class CreateItemViewModel
    {
        public Item item { get; set; }

        public List<int> tagIds { get; set; }
        public List<string> newTags { get; set; }

    }
}