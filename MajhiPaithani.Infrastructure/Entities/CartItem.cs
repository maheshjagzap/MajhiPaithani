using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class CartItem
{
    public int ICartItemId { get; set; }

    public int? ICartId { get; set; }

    public int? IProductId { get; set; }

    public int? ISellerId { get; set; }

    public int? IQuantity { get; set; }

    public decimal? DcPrice { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
