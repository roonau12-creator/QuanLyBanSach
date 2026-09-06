using BookShop.Models;
using System;
using BookShop.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Business.Services.IServices;
namespace BookShop.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task CreateCategoryAsync(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) && _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                throw new InvalidOperationException("Category name already exists.");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name) && _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != category.Id))
            {
                throw new InvalidOperationException("Category name already exists.");
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category {id} not found.");
            }
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsCategoryNameUniqueAsync(string name, int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                return !await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != categoryId.Value);
            }
            else
            {
                return !await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
            }
        }
    }
}