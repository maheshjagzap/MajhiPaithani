using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Payment
{
    public int IPaymentId { get; set; }

    public int? IOrderId { get; set; }

    public string? SPaymentMethod { get; set; }

    public decimal? DcAmount { get; set; }

    public string? STransactionId { get; set; }

    public string? SPaymentStatus { get; set; }

    public DateTime? DPaymentDate { get; set; }
}
