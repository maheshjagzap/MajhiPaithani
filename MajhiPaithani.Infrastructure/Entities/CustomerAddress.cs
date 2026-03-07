using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class CustomerAddress
{
    public int IAddressId { get; set; }

    public int? ICustomerId { get; set; }

    public string? SFullName { get; set; }

    public string? SPhoneNumber { get; set; }

    public string? SAddressLine1 { get; set; }

    public string? SAddressLine2 { get; set; }

    public string? SCity { get; set; }

    public string? SState { get; set; }

    public string? SPincode { get; set; }

    public bool? BIsDefault { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }
}
