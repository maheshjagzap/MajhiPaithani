using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/seller")]
public class SellerController : ControllerBase
{
    private readonly ISellerService _sellerService;

    public SellerController(ISellerService sellerService)
    {
        _sellerService = sellerService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Register-Seller")]
    public async Task<IActionResult> RegisterSeller(RegisterSellerRequest request)
    {
        var result = await _sellerService.RegisterSellerAsync(request);

        return Ok(result);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sellerId"></param>
    /// <returns></returns>
    [HttpGet("Get-Seller-Profile/{sellerId}")]
    public async Task<IActionResult> GetSellerProfile(int sellerId)
    {
        var result = await _sellerService.GetSellerProfileAsync(sellerId);

        return Ok(result);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sellerId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Update-Seller-Profile/{sellerId}")]
    public async Task<IActionResult> UpdateSellerProfile(int sellerId, UpdateSellerProfileRequest request)
    {
        var result = await _sellerService.UpdateSellerProfileAsync(sellerId, request);

        return Ok(result);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("add-bank-details")]
    public async Task<IActionResult> AddSellerBankDetails(AddSellerBankDetailsRequest request)
    {
        var result = await _sellerService.AddSellerBankDetailsAsync(request);

        return Ok(result);
    }
}