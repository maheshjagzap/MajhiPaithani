using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class OrderCoupon
{
    public int IOrderCouponId { get; set; }

    public int? IOrderId { get; set; }

    public int? ICouponId { get; set; }

    public decimal? DcDiscountAmount { get; set; }
}
