using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ProductTagMapping
{
    public int IMappingId { get; set; }

    public int? IProductId { get; set; }

    public int? ITagId { get; set; }
}
