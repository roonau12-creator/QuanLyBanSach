using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookShopmvc.Models;
using BookShop.Business.Services.IServices;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BookShop.Models.ViewModels;
using BookShop.Utility;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;

namespace BookShopmvc.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IApplicationUserService _applicationUserService;
    public CartController(IProductService productService, IShoppingCartService shoppingCartService,IApplicationUserService applicationUserService)
    {
        _productService = productService;
        _shoppingCartService = shoppingCartService;
        _applicationUserService = applicationUserService;
    }
    public async Task<IActionResult> Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var cartItem = await _shoppingCartService.GetUserCartItemsAsync(userId);
        var user = await _applicationUserService.GetUserByIdAsync(userId);
        ShoppingCartVM shoppingCartVM = new()
        {
            ShoppingCartList = cartItem,
            OrderHeader = new()
        };
        shoppingCartVM.OrderHeader.ApplicationUser = user;
        shoppingCartVM.OrderHeader.ApplicationUserId = userId;
        shoppingCartVM.OrderHeader.Name = user.Name;
        shoppingCartVM.OrderHeader.PhoneNumber = user.PhoneNumber ?? string.Empty;
        shoppingCartVM.OrderHeader.StreetAddress = user.StreetAddress;
        shoppingCartVM.OrderHeader.City = user.City;
        shoppingCartVM.OrderHeader.State = user.State;
        shoppingCartVM.OrderHeader.PostalCode = user.PostalCode;
        foreach (var cart in shoppingCartVM.ShoppingCartList)
        {
            shoppingCartVM.OrderHeader.OrderTotal = (cart.Price * cart.Count);
        }
        return View(shoppingCartVM);
    }
    [HttpPost]
    [ActionName("Index")]
    public async Task<IActionResult> IndexPost(ShoppingCartVM  shoppingCartVM)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var cartItem = await _shoppingCartService.GetUserCartItemsAsync(userId);
        shoppingCartVM.ShoppingCartList = cartItem;
        shoppingCartVM.OrderHeader.OrderDate = DateTime.UtcNow;
        shoppingCartVM.OrderHeader.ApplicationUserId = userId;
        foreach (var cart in shoppingCartVM.ShoppingCartList)
        {
            shoppingCartVM.OrderHeader.OrderTotal = (cart.Price * cart.Count);
        }

        shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
        
        return View(shoppingCartVM);
    }
    public async Task<IActionResult> Plus(int cartId)
    {
        var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
        if (cart != null)
        {
            if(cart.Count == 1000)
            {

            }
            else
            {
                cart.Count++;
                await _shoppingCartService.UpdateCartAsync(cart);
            }
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Minus(int cartId)
    {
        var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
        if (cart != null)
        {
            cart.Count--;
            await _shoppingCartService.UpdateCartAsync(cart);

        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Remove(int cartId)
    {
        var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
        if (cart != null)
        {
            cart.Count = 0;
            await _shoppingCartService.UpdateCartAsync(cart);
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult>UpdateCart(int cartId,int count)
    {
        var cart = await _shoppingCartService.GetCartByIdAsync(cartId);
        if(cart == null)
        {
            return NotFound();
        }
        if(count <=1)
        {
            cart.Count =0;
            await _shoppingCartService.UpdateCartAsync(cart);
            

        }
        else
        {
            cart.Count = count;
            if(count >=1000)
            {
                cart.Count =1000;
            }
            else
            {
                cart.Count = count;
            }
        }
        await _shoppingCartService.UpdateCartAsync(cart);
        return Ok(new {success = true,message = "Cart updated successfully"});
    }
    
}
