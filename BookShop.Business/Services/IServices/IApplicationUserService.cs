using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Models;

namespace BookShop.Business.Services.IServices
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}