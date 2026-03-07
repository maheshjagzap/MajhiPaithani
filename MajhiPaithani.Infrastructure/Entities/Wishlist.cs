using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Wishlist
{
    public int IWishlistId { get; set; }

    public int? IUserId { get; set; }

    public int? IProductId { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
