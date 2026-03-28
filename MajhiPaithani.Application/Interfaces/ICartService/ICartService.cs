using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;

namespace MajhiPaithani.Application.Interfaces.ICartService
{
    public interface ICartService
    {
        Task<AddToCartResponse> AddToCartAsync(AddToCartRequest request);
        Task<CartResponse> GetCartByUserIdAsync(int userId);
        Task<UpdateCartItemResponse> UpdateCartItemQuantityAsync(int cartItemId, UpdateCartItemRequest request);
        Task<RemoveCartItemResponse> RemoveCartItemAsync(int cartItemId);
    }
}
