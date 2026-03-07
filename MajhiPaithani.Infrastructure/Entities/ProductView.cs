using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ProductView
{
    public int IViewId { get; set; }

    public int? IUserId { get; set; }

    public int? IProductId { get; set; }

    public DateTime? DViewedDate { get; set; }
}
