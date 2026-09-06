using BookShop.Models;
namespace BookShop.Business.Services.IServices
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<bool>IsCategoryNameUniqueAsync(string name, int? categoryId = null);
    }
}