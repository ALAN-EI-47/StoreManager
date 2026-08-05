using System.ComponentModel.DataAnnotations;

namespace StoreManager.Web.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(255, ErrorMessage = "Name can be at most 255 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Barcode can be at most 50 characters.")]
    public string? Barcode { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cost price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Cost price must be zero or greater.")]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "Selling price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Selling price must be zero or greater.")]
    public decimal SellingPrice { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    // Populated when the API includes category data (used to display the category name in the table)
    public Category? Category { get; set; }
}