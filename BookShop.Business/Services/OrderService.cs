using BookShop.Business.Services.IServices;
using BookShop.DataAccess.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Business.Services;

public class OrderService:IOrderService
{
    private readonly ApplicationDbContext _db;
    public OrderService(ApplicationDbContext db)
    {
        _db = db;
    }
    public Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderHeader?> GetOrderByIdAsync(int id, bool includeUser = false, bool includeDetails = false)
    {
        var query = _db.orderHeaders.AsQueryable();
        if (includeUser)
        {
            query = query.Include(u => u.ApplicationUser);
        }

        if (includeDetails)
        {
            query = query.Include(u => u.OrderDetails).ThenInclude(p=>p.Product);
        }
        return await query.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<OrderHeader>> GetAllOrdersAsync(string? userId = null, string? status = null, bool includeUser = false,
        bool includeDetails = false)
    {
        var query = _db.orderHeaders.AsQueryable();
        if (includeUser)
        {
            query = query.Include(u => u.ApplicationUser);
        }

        if (includeDetails)
        {
            query = query.Include(u => u.OrderDetails).ThenInclude(p=>p.Product);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(u => u.ApplicationUserId == userId);
        }
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(u => u.OrderStatus == status);
        }

        return await query.ToListAsync();
    }
}