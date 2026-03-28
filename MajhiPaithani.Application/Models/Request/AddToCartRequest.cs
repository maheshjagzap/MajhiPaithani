namespace MajhiPaithani.Application.Models.Request
{
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? SellerId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
