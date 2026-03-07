using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Cart
{
    public int ICartId { get; set; }

    public int? ICustomerId { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DUpdatedDate { get; set; }
}
