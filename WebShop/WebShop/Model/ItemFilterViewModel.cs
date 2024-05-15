using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Model
{
    public class ItemFilterViewModel
    {
        public string SearchQuery { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<SelectListItem> Artists { get; set; }
        public List<SelectListItem> Tags { get; set; }
        public List<string> SelectedCategories { get; set; } = new List<string>();
        public List<string> SelectedArtists { get; set; } = new List<string>();
        public List<string> SelectedTags { get; set; } = new List<string>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; } 

    }
}
