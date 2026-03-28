using MajhiPaithani.Application.Interfaces.ICartService;
using MajhiPaithani.Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("add-to-cart")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var result = await _cartService.AddToCartAsync(request);
        return Ok(result);
    }

    [HttpGet("get-cart-list/{userId}")]
    public async Task<IActionResult> GetCart(int userId)
    {
        var result = await _cartService.GetCartByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpPut("update-quantity/{cartItemId}")]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] UpdateCartItemRequest request)
    {
        var result = await _cartService.UpdateCartItemQuantityAsync(cartItemId, request);
        return Ok(result);
    }

    [HttpDelete("delete-item-from-cart/{cartItemId}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var result = await _cartService.RemoveCartItemAsync(cartItemId);
        return Ok(result);
    }
}
