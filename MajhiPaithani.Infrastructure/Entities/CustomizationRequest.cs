using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class CustomizationRequest
{
    public int ICustomizationRequestId { get; set; }

    public int? ICustomerId { get; set; }

    public int? IProductId { get; set; }

    public string? SCustomizationDetails { get; set; }

    public string? SReferenceImageUrl { get; set; }

    public string? SStatus { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
