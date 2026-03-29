using MajhiPaithani.Application.Interfaces.IWishlistService;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Domain.Exceptions;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MajhiPaithani.Application.Services.WishlistService
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddToWishlistResponse> AddToWishlistAsync(AddToWishlistRequest request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.IUserId == request.UserId);
            if (!userExists)
                throw new NotFoundException("User not found.");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.IProductId == request.ProductId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            var existing = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.IUserId == request.UserId && w.IProductId == request.ProductId);

            if (existing != null)
                return new AddToWishlistResponse
                {
                    WishlistId = existing.IWishlistId,
                    ProductId = existing.IProductId ?? 0,
                    Message = "Product already in wishlist."
                };

            var wishlist = new Wishlist
            {
                IUserId = request.UserId,
                IProductId = request.ProductId,
                DCreatedDate = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return new AddToWishlistResponse
            {
                WishlistId = wishlist.IWishlistId,
                ProductId = wishlist.IProductId ?? 0,
                Message = "Product added to wishlist."
            };
        }

        public async Task<WishlistResponse> GetWishlistByUserIdAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.IUserId == userId);
            if (!userExists)
                throw new NotFoundException("User not found.");

            var wishlistItems = await (
                from w in _context.Wishlists
                join p in _context.Products on w.IProductId equals p.IProductId
                join s in _context.Sellers on p.ISellerId equals s.ISellerId into sellers
                from s in sellers.DefaultIfEmpty()
                join u in _context.Users on s.IUserId equals u.IUserId into sellerUsers
                from u in sellerUsers.DefaultIfEmpty()
                where w.IUserId == userId
                select new
                {
                    w.IWishlistId,
                    p.IProductId,
                    p.ISellerId,
                    SellerUserId = (int?)s.IUserId,
                    SellerName = u != null ? (u.SFirstName + " " + u.SLastName) : null,
                    p.ICategoryId,
                    p.SProductTitle,
                    p.SDescription,
                    p.DcBasePrice,
                    p.SColor,
                    p.SFabric,
                    p.SDesignType,
                    p.BIsCustomizationAvailable,
                    p.BIsActive,
                    p.BIsDeleted,
                    ProductCreatedDate = p.DCreatedDate,
                    ProductUpdatedDate = p.DUpdatedDate
                }
            ).ToListAsync();

            var productIds = wishlistItems.Select(x => x.IProductId).Distinct().ToList();
            var allImages = await _context.ProductImages
                .Where(pi => pi.IProductId != null && productIds.Contains(pi.IProductId.Value) && pi.isDeleted != true)
                .Select(pi => new CartProductImageDto
                {
                    IImageId = pi.IImageId,
                    SImageUrl = pi.SImageUrl,
                    BIsPrimary = pi.BIsPrimary,
                    ProductId = pi.IProductId ?? 0
                })
                .ToListAsync();

            var items = wishlistItems.Select(w => new WishlistItemDetail
            {
                WishlistId = w.IWishlistId,
                IProductId = w.IProductId,
                ISellerId = w.ISellerId,
                SellerUserId = w.SellerUserId,
                SellerName = w.SellerName,
                ICategoryId = w.ICategoryId,
                SProductTitle = w.SProductTitle,
                SDescription = w.SDescription,
                DcBasePrice = w.DcBasePrice,
                SColor = w.SColor,
                SFabric = w.SFabric,
                SDesignType = w.SDesignType,
                BIsCustomizationAvailable = w.BIsCustomizationAvailable,
                BIsActive = w.BIsActive,
                BIsDeleted = w.BIsDeleted,
                ProductCreatedDate = w.ProductCreatedDate,
                ProductUpdatedDate = w.ProductUpdatedDate,
                Images = allImages.Where(img => img.ProductId == w.IProductId).ToList()
            }).ToList();

            return new WishlistResponse { UserId = userId, Items = items };
        }

        public async Task<RemoveWishlistItemResponse> RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlist = await _context.Wishlists.FirstOrDefaultAsync(w => w.IWishlistId == wishlistId);
            if (wishlist == null)
                throw new NotFoundException("Wishlist item not found.");

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return new RemoveWishlistItemResponse { Success = true, Message = "Item removed from wishlist." };
        }
    }
}
