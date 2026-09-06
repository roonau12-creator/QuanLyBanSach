using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookShop.Models;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; } = new Product();
    [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
    public int Count { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
    [NotMapped]
    public double Price
    {
        get
        {
            if (Product == null) return 0;
            if (Count < 50)
            {
                return Product.Price;
            }
            else if (Count < 100)
            {
                return Product.Price50;
            }
            else
            {
                return Product.Price100;
            }
        }
    }

}