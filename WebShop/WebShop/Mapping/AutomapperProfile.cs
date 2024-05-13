using AutoMapper;
using DAL.Model;
using WebShop.Models.ViewModel;

namespace WebShop.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Item, CreateItemViewModel>().ReverseMap();
            CreateMap<Artist, ArtistViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
        }


    }
}
