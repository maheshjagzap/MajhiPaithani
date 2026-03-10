using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Order
{
    public int IOrderId { get; set; }
    public int UserId { get; set; }

    public int? ICustomerId { get; set; }

    public decimal? DcTotalAmount { get; set; }

    public string? SOrderStatus { get; set; }

    public string? SPaymentStatus { get; set; }

    public DateTime? DOrderDate { get; set; }

    public DateTime? DDeliveryDate { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
