using BookShop.Models;
using System;
using BookShop.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Business.Services.IServices;
namespace BookShop.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductByIdAsync(int id,bool includeCategory=false)
        {
            if (includeCategory)
            {
                return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(u=>u.Id==id);
            }
            return await _context.Products.FirstOrDefaultAsync(u=>u.Id==id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeCategory = false)
        {
            if (includeCategory)
            {
                return await _context.Products.Include(p => p.Category).ToListAsync();
            }
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product {id} not found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        
    }
}