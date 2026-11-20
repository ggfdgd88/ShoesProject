using System;
using System.Collections.Generic;

namespace ShoesProject.Models;

public partial class ProductStock
{
    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
