using DAL.Interfaces;
using DAL.Model.DTO;
using DAL.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Model;

namespace WebShop.Services
{
    public class ItemFilterService : IItemFilterService
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ITagRepository _tagRepository;

        public ItemFilterService(IItemRepository itemRepository, ICategoryRepository categoryRepository,
                                 IArtistRepository artistRepository, ITagRepository tagRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _artistRepository = artistRepository;
            _tagRepository = tagRepository;
        }

        public IList<Item> GetFilteredItems(ItemFilterViewModel filterModel)
        {
            var tags = filterModel.SelectedTags.Select(tagId => new Tag { TagId = int.Parse(tagId) }).ToList();
            var artists = filterModel.SelectedArtists.Select(artistId => new Artist { ArtistId = int.Parse(artistId) }).ToList();
            var categories = filterModel.SelectedCategories.Select(categoryId => new Category { CategoryId = int.Parse(categoryId) }).ToList();

            return _itemRepository.GetFiltered(
                tags: tags.Any() ? tags : null,
                artists: artists.Any() ? artists : null,
                categories: categories.Any() ? categories : null,
                priceMin: filterModel.PriceMin,
                priceMax: filterModel.PriceMax,
                searchQuery: filterModel.SearchQuery
            );
        }

        public ItemFilterViewModel GetFilterOptions()
        {
            var filterModel = new ItemFilterViewModel
            {
                Categories = _categoryRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList(),

                Artists = _artistRepository.GetAll().Select(a => new SelectListItem
                {
                    Value = a.ArtistId.ToString(),
                    Text = a.Name
                }).ToList(),

                Tags = _tagRepository.GetAll().Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.Name
                }).ToList()
            };

            return filterModel;
        }
    }
}
