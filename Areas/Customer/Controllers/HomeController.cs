using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookShopmvc.Models;
using BookShop.Business.Services.IServices;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookShopmvc.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    public HomeController(IProductService productService, IShoppingCartService shoppingCartService)
    {
        _productService = productService;
        _shoppingCartService = shoppingCartService;
    }
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync(includeCategory: true);
        return View(products);
    }
    public async Task<IActionResult> Details(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId, includeCategory: true);
        if (product == null)
        {
            return NotFound();
        }
        ShoppingCart cart = new()
        {
            Product = product,
            Count = 1,
            ProductId = productId
        };
        return View(cart);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        shoppingCart.ApplicationUserId = userId;
        await _shoppingCartService.AddCartToAsync(shoppingCart);
        TempData["success"] = "Item added to cart";
        return RedirectToAction("Details", new { productId = shoppingCart.ProductId });
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
