using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Business.Services.IServices;
using BookShop.DataAccess.Data;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Business.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.applicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}