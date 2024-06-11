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


        public void AddItem(int itemId, string name, decimal price, int quantity, decimal multiplier)
        {
            var existingItem = items.FirstOrDefault(i => i.ItemId == itemId);

            if (existingItem == null)
            {
                items.Add(new CartItem { ItemId = itemId, Name = name, Price = price, Quantity = quantity, SaleMultiplier =  multiplier});
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

        public decimal Total()
        {
            decimal x = 1;

            decimal total = 0;

            foreach (var item in items)
            {
                if (item.SaleMultiplier == null)
                {
                    item.SaleMultiplier = x;
                }
                total += item.Total * item.SaleMultiplier;
            }

            return total;
        }
    }
}