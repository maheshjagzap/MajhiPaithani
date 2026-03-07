using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Location
{
    public int ILocationId { get; set; }

    public string? SCity { get; set; }

    public string? SDistrict { get; set; }

    public string? SState { get; set; }

    public string? SPincode { get; set; }

    public bool? BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
