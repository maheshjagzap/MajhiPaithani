using MajhiPaithani.Application.Interfaces.ICustomerService;
using MajhiPaithani.Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/CustomerProfile")]
//[Authorize(Roles = "Customer")]
public class CustomerProfileController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerProfileController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPut("{userId}/update-customer-profile")]
    public async Task<IActionResult> UpdateProfile(int userId, UpdateCustomerRequest request)
    {
        var result = await _customerService.UpdateCustomerAsync(userId, request);
        return Ok(result);
    }

    [HttpPost("add-customer-aaddress")]
    public async Task<IActionResult> AddAddress(AddCustomerAddressRequest request)
    {
        var result = await _customerService.AddCustomerAddressAsync(request);
        return Ok(result);
    }

    [HttpPut("update-customer-address/{addressId}")]
    public async Task<IActionResult> UpdateAddress(int addressId, UpdateCustomerAddressRequest request)
    {
        var result = await _customerService.UpdateCustomerAddressAsync(addressId, request);
        return Ok(result);
    }

    [HttpGet("{userId}/addresses")]
    public async Task<IActionResult> GetAddresses(int userId)
    {
        var result = await _customerService.GetCustomerAddressesAsync(userId);
        return Ok(result);
    }
}
