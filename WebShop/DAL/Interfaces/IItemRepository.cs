using DAL.Model;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item> 
    {
        public IList<Item> GetFiltered(
     IList<Tag>? tags = null,
     IList<Artist>? artists = null,
     IList<Category>? categories = null,
     decimal? priceMin = 0,
     decimal? priceMax = null,
     string? searchQuery = null);

        public IList<Item> GetAllApi();


    }



}
