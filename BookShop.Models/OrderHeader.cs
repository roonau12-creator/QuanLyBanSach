using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BookShop.Models;

public class OrderHeader
{
    public int Id { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
    public DateTime OrderDate { get; set; }
    public DateTime ShoppingDate { get; set; }
    public double OrderTotal { get; set; }
    public string? OrderStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public string? SessiongId { get; set; }
    public string? PaymentIntentId { get; set; }
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string StreetAddress { get; set; } = string.Empty;
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string State { get; set; } = string.Empty;
    [Required]
    public string PostalCode { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [ValidateNever]
    public IEnumerable<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();


}