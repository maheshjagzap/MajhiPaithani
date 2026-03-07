using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ProductImage
{
    public int IImageId { get; set; }

    public int? IProductId { get; set; }

    public string? SImageUrl { get; set; }

    public bool? BIsPrimary { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
