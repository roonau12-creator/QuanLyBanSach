using BookShop.Models;

namespace BookShop.Business.Services.IServices;

public interface IShoppingCartService
{
    Task<ShoppingCart?> GetCartByIdAsync(int cartId);
    Task<IEnumerable<ShoppingCart>> GetUserCartItemsAsync(string userId);
    Task<int> GetCartCountAsync(string userId);
    Task<ShoppingCart> AddCartToAsync(ShoppingCart cart);
    Task UpdateCartAsync(ShoppingCart cart);
    Task ClearCartAsync(string userId);
}