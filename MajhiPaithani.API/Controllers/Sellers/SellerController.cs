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

        /// <summary>
        /// Register new seller
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.RegisterSellerAsync(request);

            return Ok(result);
        }

        /// <summary>
        /// Get seller profile
        /// </summary>
        [HttpGet("{sellerId}")]
        public async Task<IActionResult> GetSellerProfile(int sellerId)
        {
            var result = await _sellerService.GetSellerProfileAsync(sellerId);

            return Ok(result);
        }

        /// <summary>
        /// Update seller profile
        /// </summary>
        [HttpPut("{sellerId}/Update-profile")]
        public async Task<IActionResult> UpdateSellerProfile(int sellerId,[FromBody] UpdateSellerProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.UpdateSellerProfileAsync(sellerId, request);

            return Ok(result);
        }

        /// <summary>
        /// Add seller bank details
        /// </summary>
        [HttpPost("bank-details")]
        public async Task<IActionResult> AddSellerBankDetails([FromBody] AddSellerBankDetailsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sellerService.AddSellerBankDetailsAsync(request);

            return Ok(result);
        }
        /// <summary>
        /// Get seller bank details
        /// </summary>
        [HttpGet("{sellerId}/bank-details")]
        public async Task<IActionResult> GetSellerBankDetails(int sellerId)
        {
            var result = await _sellerService.GetSellerBankDetailsAsync(sellerId);

            return Ok(result);
        }
        /// <summary>
        /// Update seller bank details
        /// </summary>
        [HttpPut("bank-details")]
        public async Task<IActionResult> UpdateSellerBankDetails([FromBody] UpdateBankDetailsRequest request)
        {
            var result = await _sellerService.UpdateSellerBankDetailsAsync(request);

            return Ok(result);
        }
        /// <summary>
        /// Upload seller profile image
        /// </summary>
        [HttpPost("profile/upload-image")]
        public async Task<IActionResult> UploadSellerProfileImage([FromForm] UploadSellerProfileImageRequest request)
        {
            var result = await _sellerService.UploadSellerProfileImageAsync(request);

            return Ok(result);
        }
        /// <summary>
        /// Update seller shop details
        /// </summary>
        [HttpPut("shop-details")]
        public async Task<IActionResult> UpdateSellerShopDetails([FromBody] UpdateShopDetailsRequest request)
        {
            var result = await _sellerService.UpdateSellerShopDetailsAsync(request);

            return Ok(result);
        }
        /// <summary>
        /// Upload seller design
        /// </summary>
        [HttpPost("add-designs")]
        public async Task<IActionResult> AddDesign([FromForm] AddDesignRequest request)
        {
            var result = await _sellerService.AddDesignAsync(request);

            return Ok(result);
        }
        /// <summary>
        /// Get all designs of a seller
        /// </summary>
        [HttpGet("{sellerId}/designs")]
        public async Task<IActionResult> GetSellerDesigns(int sellerId)
        {
            var result = await _sellerService.GetSellerDesignsAsync(sellerId);

            return Ok(result);
        }
        /// <summary>
        /// Update seller design
        /// </summary>
        [HttpPut("designs/{designId}")]
        public async Task<IActionResult> UpdateDesign(
            int designId,
            [FromForm] UpdateDesignRequest request)
        {
            var result = await _sellerService.UpdateDesignAsync(designId, request);

            return Ok(result);
        }
    }
}