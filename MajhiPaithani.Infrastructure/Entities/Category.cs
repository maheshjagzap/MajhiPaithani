using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Category
{
    public int ICategoryId { get; set; }

    public string? SCategoryName { get; set; }

    public string? SDescription { get; set; }

    public bool? BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }
}
