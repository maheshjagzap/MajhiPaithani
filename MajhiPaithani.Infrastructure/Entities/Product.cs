using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Product
{
    public int IProductId { get; set; }

    public int? ISellerId { get; set; }

    public int? ICategoryId { get; set; }

    public string? SProductTitle { get; set; }

    public string? SDescription { get; set; }

    public decimal? DcBasePrice { get; set; }

    public string? SColor { get; set; }

    public string? SFabric { get; set; }

    public string? SDesignType { get; set; }

    public bool? BIsCustomizationAvailable { get; set; }

    public bool? BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DUpdatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }
}
