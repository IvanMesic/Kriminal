using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ServiceInterfaces
{
    public interface ICartService
    {
        Cart GetCart();
        void AddItem(int itemId, string name, decimal price, int quantity, decimal multiplier);
        void RemoveItem(int itemId);
        void ClearCart();
        decimal GetTotal();
    }
}
