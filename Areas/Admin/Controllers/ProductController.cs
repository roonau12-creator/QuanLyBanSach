using Microsoft.AspNetCore.Mvc;
using BookShop.DataAccess.Data;
using BookShop.Business.Services.IServices;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookShop.Models.ViewModels;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using BookShop.Utility;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookShop.Controllers
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    [Area("Admin")]
    [Authorize(Roles =SD.RoleAdmin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService,IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment=webHostEnvironment;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
          public async Task<IActionResult> Upsert(int? id)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ProductVM productVM = new()
            {
                CategoryList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Product = new Product()
            };
            if(id==null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                productVM.Product = await _productService.GetProductByIdAsync(id.Value) ?? new Product();
                return View(productVM);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Upsert")]
        public async Task<IActionResult> UpsertPOST(ProductVM productVM, IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine("images", "products");
                    string finalPath = Path.Combine(wwwRootPath, productPath);


                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    //save the new image
                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = "/" + Path.Combine(productPath, fileName).Replace("\\", "/");
                }

                if (productVM.Product.Id == 0)
                {
                    //create
                    await _productService.CreateProductAsync(productVM.Product);
                }
                else
                {
                    await _productService.UpdateProductAsync(productVM.Product);
                    
                }

                
                TempData["success"] = productVM.Product.Id == 0 ? "Product created successfully" : "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                productVM = new()
                {
                    CategoryList = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };
                return View(productVM);
            }

        }
        
        #region API CALLS
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var products =await _productService.GetAllProductsAsync(true);
            return Json(new { data = products });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id ==0)
            {
                return Json(new {success =false, message = "Invalid id"});
            }
            var productToBeDelete = await _productService.GetProductByIdAsync(id.Value);
            if (productToBeDelete == null)
            {
                return Json(new {success = false, message = "Error while deleting"});
            }
            if (!string.IsNullOrEmpty(productToBeDelete.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,productToBeDelete.ImageUrl.TrimStart('\\','/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            await _productService.DeleteProductAsync(id.Value);
            TempData["success"] = "Product deleted successfully.";
            return Json(new {success = true, message = "Delete Successfully"});
        }
        #endregion      
        
        
        
    }
}