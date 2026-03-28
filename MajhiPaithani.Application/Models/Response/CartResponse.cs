namespace MajhiPaithani.Application.Models.Response
{
    public class AddToCartResponse
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class CartItemDetail
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string? ProductTitle { get; set; }
        public string? ProductImage { get; set; }
        public int? SellerId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CartResponse
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
        public List<CartItemDetail> Items { get; set; } = new();
        public decimal TotalAmount => Items.Where(i => i.IsAvailable).Sum(i => i.PriceAtTime * i.Quantity);
    }

    public class UpdateCartItemResponse
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RemoveCartItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
