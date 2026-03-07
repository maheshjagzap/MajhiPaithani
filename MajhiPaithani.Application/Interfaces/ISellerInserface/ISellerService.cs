using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
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



    }
}
