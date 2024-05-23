using DAL.Model;
using DAL.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace DAL.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                var newCart = new Cart();
                SetCart(newCart);
                return newCart;
            }

            var cart = JsonConvert.DeserializeObject<Cart>(cartJson) ?? new Cart();
            return cart;
        }

        public void AddItem(int itemId, string name, decimal price, int quantity, decimal multiplier)
        {
            var cart = GetCart();
            cart.AddItem(itemId, name, price, quantity, multiplier);
            SetCart(cart);
        }

        public void RemoveItem(int itemId)
        {
            var cart = GetCart();
            cart.RemoveItem(itemId);
            SetCart(cart);
        }

        public void ClearCart()
        {
            var cart = GetCart();
            cart.Clear();
            SetCart(cart);
        }

        public decimal GetTotal()
        {
            var cart = GetCart();
            return cart.Total();
        }

        private void SetCart(Cart cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            var cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }
    }

    public static class SessionExtensions
    {
        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, System.Text.Encoding.UTF8.GetBytes(value));
        }

        public static string GetString(this ISession session, string key)
        {
            session.TryGetValue(key, out var data);
            return data == null ? null : System.Text.Encoding.UTF8.GetString(data);
        }
    }
}
