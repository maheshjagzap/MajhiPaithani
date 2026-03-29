namespace MajhiPaithani.Application.Models.Response
{
    public class AddToWishlistResponse
    {
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class WishlistItemDetail
    {
        public int WishlistId { get; set; }
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

    public class WishlistResponse
    {
        public int UserId { get; set; }
        public List<WishlistItemDetail> Items { get; set; } = new();
    }

    public class RemoveWishlistItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
