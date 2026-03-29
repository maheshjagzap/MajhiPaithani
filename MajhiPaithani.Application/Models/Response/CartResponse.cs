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
        public int CartItemCount { get; set; }
    }

    public class CartProductImageDto
    {
        public int IImageId { get; set; }
        public string? SImageUrl { get; set; }
        public bool? BIsPrimary { get; set; }
        internal int ProductId { get; set; }  // used for grouping, not serialized
    }

    public class CartItemDetail
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public bool IsAvailable { get; set; }

        // Product details
        public int IProductId { get; set; }
        public int? ISellerId { get; set; }
        public int? SellerUserId { get; set; }
        public string? SellerName { get; set; }
        public int? ICategoryId { get; set; }
        public string? SProductTitle { get; set; }
        public string? SDescription { get; set; }
        public decimal? DcBasePrice { get; set; }
        public string? SColor { get; set; }
        public string? SFabric { get; set; }
        public string? SDesignType { get; set; }
        public bool? BIsCustomizationAvailable { get; set; }
        public bool? BIsActive { get; set; }
        public bool? BIsDeleted { get; set; }
        public DateTime? ProductCreatedDate { get; set; }
        public DateTime? ProductUpdatedDate { get; set; }
        public List<CartProductImageDto> Images { get; set; } = new();
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
        public int CartItemCount { get; set; }
    }
}
