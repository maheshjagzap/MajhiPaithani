using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class SellerDocument
{
    public int IDocumentId { get; set; }

    public int? ISellerId { get; set; }

    public string? SDocumentType { get; set; }

    public string? SDocumentUrl { get; set; }

    public bool? BIsVerified { get; set; }

    public DateTime? DUploadedDate { get; set; }
}
