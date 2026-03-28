namespace MajhiPaithani.Infrastructure.Entities;

public class CartItem
{
    public int CartItemId { get; set; }
    public int ICartId { get; set; }
    public int IProductId { get; set; }
    public int? ISellerId { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtTime { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime DtCreated { get; set; }
    public DateTime? DtUpdated { get; set; }
}
