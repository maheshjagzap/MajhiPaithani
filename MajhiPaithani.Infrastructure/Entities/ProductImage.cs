using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ProductImage
{
    [Key]
    public int IImageId { get; set; }

    public int? IProductId { get; set; }

    public string? SImageUrl { get; set; }

    public bool? BIsPrimary { get; set; }

    public int sellerId { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
