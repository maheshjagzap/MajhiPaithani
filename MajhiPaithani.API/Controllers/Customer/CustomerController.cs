using MajhiPaithani.Application.Interfaces.ICustomerService;
using MajhiPaithani.Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customer")]
//[Authorize(Roles = "Customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
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
}
