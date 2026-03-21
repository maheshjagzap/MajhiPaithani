using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class SellerBankDetail
{
    public int IbankDetailId { get; set; }

    public int? IsellerId { get; set; }

    public string? SaccountHolderName { get; set; }

    public string? SaccountNumber { get; set; }

    public string? Sifsccode { get; set; }

    public string? SbankName { get; set; }

    public DateTime? DcreatedDate { get; set; }

    public DateTime? DupdatedDate { get; set; }
}
