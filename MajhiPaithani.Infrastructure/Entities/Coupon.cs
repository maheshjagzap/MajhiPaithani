using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Coupon
{
    public int ICouponId { get; set; }

    public string? SCouponCode { get; set; }

    public decimal? DcDiscountAmount { get; set; }

    public DateTime? DStartDate { get; set; }

    public DateTime? DEndDate { get; set; }

    public bool? BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
