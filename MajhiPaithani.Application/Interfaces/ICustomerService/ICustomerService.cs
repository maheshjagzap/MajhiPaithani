using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;

namespace MajhiPaithani.Application.Interfaces.ICustomerService
{
    public interface ICustomerService
    {
        Task<UpdateCustomerResponse> UpdateCustomerAsync(int userId, UpdateCustomerRequest request);
        Task<AddCustomerAddressResponse> AddCustomerAddressAsync(AddCustomerAddressRequest request);
        Task<UpdateCustomerAddressResponse> UpdateCustomerAddressAsync(int addressId, UpdateCustomerAddressRequest request);
        Task<List<GetCustomerAddressesResponse>> GetCustomerAddressesAsync(int userId);
    }
}
