using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class AdminLog
{
    public int ILogId { get; set; }

    public int? IAdminUserId { get; set; }

    public string? SAction { get; set; }

    public string? SDescription { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
