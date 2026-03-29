using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;

namespace MajhiPaithani.Application.Interfaces.IWishlistService
{
    public interface IWishlistService
    {
        Task<AddToWishlistResponse> AddToWishlistAsync(AddToWishlistRequest request);
        Task<WishlistResponse> GetWishlistByUserIdAsync(int userId);
        Task<RemoveWishlistItemResponse> RemoveFromWishlistAsync(int wishlistId);
    }
}
