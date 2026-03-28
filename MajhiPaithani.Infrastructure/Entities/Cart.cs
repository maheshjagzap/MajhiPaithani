namespace MajhiPaithani.Infrastructure.Entities;

public class Cart
{
    public int CartId { get; set; }
    public int IUserId { get; set; }
    public string? Status { get; set; }
    public DateTime DtCreated { get; set; }
    public DateTime? DtUpdated { get; set; }
}
