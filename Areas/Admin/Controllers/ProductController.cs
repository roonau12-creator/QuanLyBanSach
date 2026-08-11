using Microsoft.AspNetCore.Mvc;
using BookShop.DataAccess.Data;
using BookShop.Business.Services.IServices;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookShop.Models;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookShop.Controllers
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        
        public async Task<IActionResult> Upsert()
        {
            IEnumerable<SelectListItem> categoryList =(await _categoryService.GetAllCategoriesAsync()).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;
            return View(categoryList);
        }
        [ValidateAntiForgeryToken]
        [ActionName("Upsert")]
        [HttpPost]
        public async Task<IActionResult> UpsertPost(Product product)
        {
            if (ModelState.IsValid)
            {
               
                await _productService.CreateProductAsync(product);
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productService.DeleteProductAsync(product.Id);
            TempData["success"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }
        #region API CALLS
        public async Task<IActionResult> GetAll()
        {
            var products =await _productService.GetAllProductsAsync(true);
            return Json(new { data = products });
        }
        #endregion      
        
        
        
    }
}