using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IReadOnlyCollection<CartItem> Items
        {
            get => items.AsReadOnly();
            set => items = value.ToList(); 
        }

        public void AddItem(int itemId, string name, decimal price, int quantity)
        {
            var existingItem = items.FirstOrDefault(i => i.ItemId == itemId);

            if (existingItem == null)
            {
                items.Add(new CartItem { ItemId = itemId, Name = name, Price = price, Quantity = quantity });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int itemId)
        {
            items.RemoveAll(i => i.ItemId == itemId);
        }

        public void Clear() => items.Clear();

        public decimal Total => items.Sum(i => i.Total);
    }
}