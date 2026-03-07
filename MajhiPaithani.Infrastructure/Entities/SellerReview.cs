using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class SellerReview
{
    public int ISellerReviewId { get; set; }

    public int? ISellerId { get; set; }

    public int? ICustomerId { get; set; }

    public int? IRating { get; set; }

    public string? SComment { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
