using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Api.Models;

[Table("products")]
public class Product
{
    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Barcode { get; set; }

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal CostPrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal SellingPrice { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation property — lets EF Core load the related Category via .Include(p => p.Category)
    public Category? Category { get; set; }
}