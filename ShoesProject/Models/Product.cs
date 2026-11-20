using System;
using System.Collections.Generic;

namespace ShoesProject.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? SubcategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    public string? Description { get; set; }

    public string? Composition { get; set; }

    public string? ImagePath { get; set; }

    public decimal Price { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual Subcategory? Subcategory { get; set; }
}
