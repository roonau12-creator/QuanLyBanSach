using Microsoft.AspNetCore.Mvc;
using BookShop.DataAccess.Data;
using BookShop.Business.Services.IServices;
using BookShop.Models;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace BookShop.Controllers
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _context;
        public CategoryController(ICategoryService context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories =await _context.GetAllCategoriesAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(Category category)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(category.Name) && !(await _context.IsCategoryNameUniqueAsync(category.Name)))
                {
                    ModelState.AddModelError("Name", "Category name already exists.");
                    return View(category);
                }
                await _context.CreateCategoryAsync(category);
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = await _context.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdatePost(Category category)  
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(category.Name) && !(await _context.IsCategoryNameUniqueAsync(category.Name, category.Id)))
                {
                    ModelState.AddModelError("Name", "Category name already exists.");
                    return View(category);
                }
                await _context.UpdateCategoryAsync(category);
                TempData["success"] = "Category updated successfully.";
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
            var category = await _context.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var category = await _context.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _context.DeleteCategoryAsync(category.Id);
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
                 
        
        
        
    }
}