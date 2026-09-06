using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Models.ViewModels;
public class RegisterVM
{
    [Required]
    [EmailAddress]
    public string Email {get;set;} = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password {get;set;} = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    [Display(Name ="Confirm Password")]
    public string ConfirmPassword {get;set;} = string.Empty;
    [Required]
    public string? Name {get;set;}
    public string? StreetAddress {get;set;}
    public string? City {get;set;}
    public string? State {get;set;}
    public string? PostalCode {get;set;}
    [Display(Name="Phone Number")]
    public string PhoneNumber {get;set;} = string.Empty;
    public string? Role {get;set;}
    [ValidateNever]
    public IEnumerable<SelectListItem>RoleList{get;set;} = new List<SelectListItem>();



}