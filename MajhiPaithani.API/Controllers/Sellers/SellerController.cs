using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MajhiPaithani.API.Controllers
{
    [ApiController]
    [Route("api/seller")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }


        [HttpPost("register-seller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.RegisterSellerAsync(request);

            return Ok(result);
        }


        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetSellerProfile(int sellerId)
        {
            var result = await _sellerService.GetSellerProfileAsync(sellerId);

            return Ok(result);
        }

        [HttpPut("{sellerId}/Update-profile")]
        public async Task<IActionResult> UpdateSellerProfile(int sellerId,[FromBody] UpdateSellerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.UpdateSellerProfileAsync(sellerId, request);

            return Ok(result);
        }


        [HttpPost("bank-details")]
        public async Task<IActionResult> AddSellerBankDetails([FromBody] AddSellerBankDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.AddSellerBankDetailsAsync(request);

            return Ok(result);
        }

        [HttpGet("{sellerId}/bank-details")]
        public async Task<IActionResult> GetSellerBankDetails(int sellerId)
        {
            var result = await _sellerService.GetSellerBankDetailsAsync(sellerId);

            return Ok(result);
        }

        [HttpPut("bank-details")]
        public async Task<IActionResult> UpdateSellerBankDetails([FromBody] UpdateBankDetailsRequest request)
        {
            var result = await _sellerService.UpdateSellerBankDetailsAsync(request);

            return Ok(result);
        }

        [HttpPost("profile/upload-image")]
        public async Task<IActionResult> UploadSellerProfileImage([FromForm] UploadSellerProfileImageRequest request)
        {
            var result = await _sellerService.UploadSellerProfileImageAsync(request);

            return Ok(result);
        }

        [HttpPut("shop-details")]
        public async Task<IActionResult> UpdateSellerShopDetails([FromBody] UpdateShopDetailsRequest request)
        {
            var result = await _sellerService.UpdateSellerShopDetailsAsync(request);

            return Ok(result);
        }

        [HttpPost("add-designs")]
        public async Task<IActionResult> AddDesign([FromForm] AddDesignRequest request)
        {
            var result = await _sellerService.AddDesignAsync(request);

            return Ok(result);
        }

        [HttpGet("{sellerId}/Get-all-designs")]
        public async Task<IActionResult> GetSellerDesigns(int sellerId)
        {
            var result = await _sellerService.GetSellerDesignsAsync(sellerId);

            return Ok(result);
        }

        [HttpPut("update-designs/{designId}")]
        public async Task<IActionResult> UpdateDesign(
            int designId,
            [FromForm] UpdateDesignRequest request)
        {
            var result = await _sellerService.UpdateDesignAsync(designId, request);

            return Ok(result);
        }

        [HttpDelete("designs/{designId}")]
        public async Task<IActionResult> DeleteDesign(int designId)
        {
            var result = await _sellerService.DeleteDesignAsync(designId);

            return Ok(result);
        }

        [HttpGet("designs/{designId}")]
        public async Task<IActionResult> GetDesignDetails(int designId)
        {
            var result = await _sellerService.GetDesignDetailsAsync(designId);

            return Ok(result);
        }

        [HttpPost("products")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            var result = await _sellerService.AddProductAsync(request);

            return Ok(result);
        }

        [HttpPut("products/{productId}")]
        public async Task<IActionResult> UpdateProduct(
            int productId,
            [FromBody] UpdateProductRequest request)
        {
            var result = await _sellerService.UpdateProductAsync(productId, request);

            return Ok(result);
        }

        [HttpDelete("products/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _sellerService.DeleteProductAsync(productId);

            return Ok(result);
        }
    }
}