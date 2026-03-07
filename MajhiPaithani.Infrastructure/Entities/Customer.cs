using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Customer
{
    public int ICustomerId { get; set; }

    public int? IUserId { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }
}
