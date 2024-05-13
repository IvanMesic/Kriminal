namespace WebShop.Models.ViewModel
{
    public class ArtistViewModel
    {
        public string Name { get; set; }
        public string Bio { get; set; }

        public virtual ICollection<CreateItemViewModel> Items { get; set; }
    }
}
