using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.UpdateBankDetailsResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Interfaces.ISellerInserface
{
    public interface ISellerService
    {
        Task<GetSellerProfileResponse> GetSellerProfileAsync(int sellerId);
        Task<string> UpdateSellerProfileAsync(int sellerId, UpdateSellerProfileRequest request);
        Task<RegisterSellerResponse> RegisterSellerAsync(RegisterSellerRequest request);
        Task<AddSellerBankDetailsResponse> AddSellerBankDetailsAsync(AddSellerBankDetailsRequest request);
        Task<GetSellerBankDetailsResponse> GetSellerBankDetailsAsync(int sellerId);
        Task<UpdateBankDetailsResponse> UpdateSellerBankDetailsAsync(UpdateBankDetailsRequest request);
        Task<UploadSellerProfileImageResponse> UploadSellerProfileImageAsync(UploadSellerProfileImageRequest request);
        Task<UpdateShopDetailsResponse> UpdateSellerShopDetailsAsync(UpdateShopDetailsRequest request);
        Task<AddDesignResponse> AddDesignAsync(AddDesignRequest request);
        Task<List<GetSellerDesignsResponse>> GetSellerDesignsAsync(int sellerId);
        Task<UpdateDesignResponse> UpdateDesignAsync(int designId, UpdateDesignRequest request);
        Task<DeleteDesignResponse> DeleteDesignAsync(int designId);
        Task<DesignDetailsResponse> GetDesignDetailsAsync(int designId);
        Task<AddProductResponse> AddProductAsync(AddProductRequest request);
        Task<UpdateProductResponse> UpdateProductAsync(int productId, UpdateProductRequest request);
        Task<DeleteProductResponse> DeleteProductAsync(int productId);
    }
}