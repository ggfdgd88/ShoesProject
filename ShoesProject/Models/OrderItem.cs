using System;
using System.Collections.Generic;

namespace ShoesProject.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ProductStock ProductStock { get; set; } = null!;
}
