using MajhiPaithani.Application.Interfaces.ICartService;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Domain.Exceptions;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MajhiPaithani.Application.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddToCartResponse> AddToCartAsync(AddToCartRequest request)
        {
            if (request.Quantity <= 0)
                throw new BadRequestException("Quantity must be greater than zero.");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.IProductId == request.ProductId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            var userExists = await _context.Users.AnyAsync(u => u.IUserId == request.UserId);
            if (!userExists)
                throw new NotFoundException("User not found.");

            // Get or create active cart
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.IUserId == request.UserId && c.Status == "Active");

            if (cart == null)
            {
                cart = new Cart
                {
                    IUserId = request.UserId,
                    Status = "Active",
                    DtCreated = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Check if product already exists in cart
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ICartId == cart.CartId && ci.IProductId == request.ProductId);

            string message;
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.DtUpdated = DateTime.UtcNow;
                message = "Cart item quantity updated.";
            }
            else
            {
                existingItem = new CartItem
                {
                    ICartId = cart.CartId,
                    IProductId = request.ProductId,
                    ISellerId = request.SellerId,
                    Quantity = request.Quantity,
                    PriceAtTime = product.DcBasePrice ?? 0,
                    IsAvailable = true,
                    DtCreated = DateTime.UtcNow
                };
                _context.CartItems.Add(existingItem);
                message = "Product added to cart.";
            }

            await _context.SaveChangesAsync();

            var cartItemCount = await _context.CartItems.CountAsync(ci => ci.ICartId == cart.CartId);

            return new AddToCartResponse
            {
                CartId = cart.CartId,
                CartItemId = existingItem.CartItemId,
                ProductId = existingItem.IProductId,
                Quantity = existingItem.Quantity,
                PriceAtTime = existingItem.PriceAtTime,
                Message = message,
                CartItemCount = cartItemCount
            };
        }

        public async Task<CartResponse> GetCartByUserIdAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.IUserId == userId);
            if (!userExists)
                throw new NotFoundException("User not found.");

            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.IUserId == userId && c.Status == "Active");

            if (cart == null)
                return new CartResponse { UserId = userId, Status = "Empty", Items = new() };

            var cartItems = await (
                from ci in _context.CartItems
                join p in _context.Products on ci.IProductId equals p.IProductId
                join s in _context.Sellers on p.ISellerId equals s.ISellerId into sellers
                from s in sellers.DefaultIfEmpty()
                join u in _context.Users on s.IUserId equals u.IUserId into sellerUsers
                from u in sellerUsers.DefaultIfEmpty()
                where ci.ICartId == cart.CartId
                select new
                {
                    ci.CartItemId,
                    ci.Quantity,
                    ci.PriceAtTime,
                    ci.IsAvailable,
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

            // Fetch all images for the products in this cart in one query
            var productIds = cartItems.Select(x => x.IProductId).Distinct().ToList();
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

            var items = cartItems.Select(ci => new CartItemDetail
            {
                CartItemId = ci.CartItemId,
                Quantity = ci.Quantity,
                PriceAtTime = ci.PriceAtTime,
                IsAvailable = ci.IsAvailable,
                IProductId = ci.IProductId,
                ISellerId = ci.ISellerId,
                SellerUserId = ci.SellerUserId,
                SellerName = ci.SellerName,
                ICategoryId = ci.ICategoryId,
                SProductTitle = ci.SProductTitle,
                SDescription = ci.SDescription,
                DcBasePrice = ci.DcBasePrice,
                SColor = ci.SColor,
                SFabric = ci.SFabric,
                SDesignType = ci.SDesignType,
                BIsCustomizationAvailable = ci.BIsCustomizationAvailable,
                BIsActive = ci.BIsActive,
                BIsDeleted = ci.BIsDeleted,
                ProductCreatedDate = ci.ProductCreatedDate,
                ProductUpdatedDate = ci.ProductUpdatedDate,
                Images = allImages.Where(img => img.ProductId == ci.IProductId).ToList()
            }).ToList();

            return new CartResponse
            {
                CartId = cart.CartId,
                UserId = cart.IUserId,
                Status = cart.Status,
                Items = items
            };
        }

        public async Task<UpdateCartItemResponse> UpdateCartItemQuantityAsync(int cartItemId, UpdateCartItemRequest request)
        {
            if (request.Quantity < 0)
                throw new BadRequestException("Quantity cannot be negative.");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
            if (cartItem == null)
                throw new NotFoundException("Cart item not found.");

            if (request.Quantity == 0)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return new UpdateCartItemResponse
                {
                    CartItemId = cartItemId,
                    Quantity = 0,
                    Message = "Item removed from cart as quantity reached zero."
                };
            }

            cartItem.Quantity = request.Quantity;
            cartItem.DtUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new UpdateCartItemResponse
            {
                CartItemId = cartItem.CartItemId,
                Quantity = cartItem.Quantity,
                Message = "Cart item quantity updated."
            };
        }

        public async Task<RemoveCartItemResponse> RemoveCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
            if (cartItem == null)
                throw new NotFoundException("Cart item not found.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            var cartItemCount = await _context.CartItems.CountAsync(ci => ci.ICartId == cartItem.ICartId);

            return new RemoveCartItemResponse { Success = true, Message = "Item removed from cart.", CartItemCount = cartItemCount };
        }
    }
}
