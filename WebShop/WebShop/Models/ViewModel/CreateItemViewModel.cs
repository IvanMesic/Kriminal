using DAL.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.ViewModel
{
    public class CreateItemViewModel
    {
        public Item item { get; set; }
        
        public List<int> tagIds { get; set; }

    }
}