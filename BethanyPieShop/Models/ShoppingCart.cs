using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        public string ShopingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        private ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<AppDbContext>();

            string cartId = session.GetString("cartId") ?? Guid.NewGuid().ToString();

            session.SetString("cartId", cartId);

            return new ShoppingCart(context) { ShopingCartId = cartId };
        }

        public void AddToCart(Pie pie, int amount)
        {
            var shopingCartItem = _appDbContext.ShopingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShopingCartId == ShopingCartId);

            if (shopingCartItem == null)
            {
                shopingCartItem = new ShoppingCartItem
                {
                    ShopingCartId = ShopingCartId,
                    Pie = pie,
                    Amount = 1,
                };
                _appDbContext.ShopingCartItems.Add(shopingCartItem);
            }
            else
            {
                shopingCartItem.Amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Pie pie)
        {
            var shopingCartItem = _appDbContext.ShopingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShopingCartId == ShopingCartId);
            var localAmount = 0;

            if (shopingCartItem == null)
            {
                if (shopingCartItem.Amount > 1)
                {
                    shopingCartItem.Amount--;
                    localAmount = shopingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShopingCartItems.Remove(shopingCartItem);
                }
            }
            _appDbContext.SaveChanges();
            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _appDbContext.ShopingCartItems.Where(c => c.ShopingCartId == ShopingCartId).Include(s => s.Pie).ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext.ShopingCartItems.Where(c => c.ShopingCartId == ShopingCartId);
            _appDbContext.ShopingCartItems.RemoveRange(cartItems);
            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShopingCartItems.Where(c => c.ShopingCartId == ShopingCartId).Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }
    }
}