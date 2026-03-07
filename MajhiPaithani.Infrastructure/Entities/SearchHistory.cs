using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class SearchHistory
{
    public int ISearchId { get; set; }

    public int? IUserId { get; set; }

    public string? SSearchKeyword { get; set; }

    public DateTime? DSearchDate { get; set; }
}
