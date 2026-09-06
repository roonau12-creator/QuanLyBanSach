using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace BookShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ISBN { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000.")]
        public double ListPrice { get; set; }
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        [ValidateNever]
        [Display(Name = "Product Image")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}