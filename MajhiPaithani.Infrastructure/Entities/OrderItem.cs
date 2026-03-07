using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class OrderItem
{
    public int IOrderItemId { get; set; }

    public int? IOrderId { get; set; }

    public int? IProductId { get; set; }

    public int? ISellerId { get; set; }

    public int? IQuantity { get; set; }

    public decimal? DcPrice { get; set; }

    public decimal? DcTotalPrice { get; set; }
}
