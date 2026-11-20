using System;
using System.Collections.Generic;

namespace ShoesProject.Models;

public partial class Size
{
    public int Id { get; set; }

    public string SizeValue { get; set; } = null!;

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
