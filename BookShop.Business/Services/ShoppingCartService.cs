using BookShop.Business.Services.IServices;
using BookShop.DataAccess.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Business.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly ApplicationDbContext _context;
    public ShoppingCartService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<ShoppingCart> AddCartToAsync(ShoppingCart cart)
    {
        var existingItem = await _context.shoppingCarts.Include(u => u.Product).FirstOrDefaultAsync(u => u.ApplicationUserId == cart.ApplicationUserId && u.ProductId == cart.ProductId);
        if (existingItem != null)
        {
            existingItem.Count += cart.Count;
            await _context.SaveChangesAsync();
            return existingItem;
        }
        else
        {
            _context.shoppingCarts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var cartItems = await _context.shoppingCarts.Include(p => p.Product).Where(u => u.ApplicationUserId == userId).ToListAsync();
        if (cartItems.Any())
        {
            _context.shoppingCarts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ShoppingCart?> GetCartByIdAsync(int cartId)
    {
        return await _context.shoppingCarts.Include(p => p.Product).FirstOrDefaultAsync(u => u.Id == cartId);
    }

    public async Task<int> GetCartCountAsync(string userId)
    {
        return await _context.shoppingCarts.Where(u => u.ApplicationUserId == userId).SumAsync(u => u.Count);
    }

    public async Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId)
    {
        return await _context.shoppingCarts.Include(p => p.Product).Where(u => u.ApplicationUserId == userId).ToListAsync();
    }

    public async Task UpdateCartAsync(ShoppingCart cart)
    {
        if (cart.Count < 0)
        {
            _context.shoppingCarts.Remove(cart);
        }
        else
        {
            _context.shoppingCarts.Update(cart);
        }
        await _context.SaveChangesAsync();
    }
}