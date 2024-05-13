using AutoMapper;
using DAL.Model;
using WebShop.Model;

namespace WebShop.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Item, CreateItemViewModel>().ReverseMap();
            CreateMap<Artist, CreateArtistViewModel>().ReverseMap();
        }
    }
}
