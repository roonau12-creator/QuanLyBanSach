using Microsoft.EntityFrameworkCore;
using BookShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BookShop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    ISBN = "SWD9999001",
                    Author = "Billy Spark",
                    ListPrice = 99.0,
                    Price = 90.0,
                    Price50 = 85.0,
                    Price100 = 80.0,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    ISBN = "CAW777777701",
                    Author = "Nancy Hoover",
                    ListPrice = 40.0,
                    Price = 30.0,
                    Price50 = 25.0,
                    Price100 = 20.0,
                    CategoryId = 2,
                    ImageUrl = ""
                }
            );
        }
    }
}
