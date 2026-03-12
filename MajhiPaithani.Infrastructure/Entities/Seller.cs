using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Seller
{
    public int ISellerId { get; set; }

    public int IUserId { get; set; }

    public string? SShopName { get; set; }

    public string? SShopDescription { get; set; }

    public string? SShopAddress { get; set; }

    public string? SCity { get; set; }

    public string? SState { get; set; }

    public string? SPincode { get; set; }

    public string? SBusinessDescription { get; set; }

    public string? SProfileImageUrl { get; set; }

    public int ILocationId { get; set; }

    public bool? BIsVerified { get; set; }

    public bool BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DUpdatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }

    // Navigation Properties (optional but recommended)

    public virtual ICollection<SellerBankDetail> SellerBankDetails { get; set; } = new List<SellerBankDetail>();
}