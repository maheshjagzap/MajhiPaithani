using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Role
{
    public int IRoleId { get; set; }

    public string SRoleName { get; set; } = null!;

    public bool? BIsActive { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
