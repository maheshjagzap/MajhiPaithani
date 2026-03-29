using MajhiPaithani.Application.Interfaces.ICustomerService;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Domain.Exceptions;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MajhiPaithani.Application.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateCustomerResponse> UpdateCustomerAsync(int userId, UpdateCustomerRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.IUserId == userId);
            if (user == null)
                throw new NotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(request.FirstName)) user.SFirstName = request.FirstName;
            if (!string.IsNullOrWhiteSpace(request.LastName)) user.SLastName = request.LastName;
            if (!string.IsNullOrWhiteSpace(request.Email)) user.SEmail = request.Email;
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber)) user.SPhoneNumber = request.PhoneNumber;
            user.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateCustomerResponse
            {
                UserId = user.IUserId,
                FirstName = user.SFirstName,
                LastName = user.SLastName,
                Email = user.SEmail,
                PhoneNumber = user.SPhoneNumber
            };
        }

        public async Task<AddCustomerAddressResponse> AddCustomerAddressAsync(AddCustomerAddressRequest request)
        {
            var userExists = await _context.Users.AnyAsync(x => x.IUserId == request.UserId);
            if (!userExists)
                throw new NotFoundException("User not found");

            // If new address is default, unset existing default
            if (request.IsDefault)
            {
                var existing = await _context.UserAddresses
                    .Where(x => x.UserId == request.UserId && x.IsDefault == true)
                    .ToListAsync();
                existing.ForEach(a => a.IsDefault = false);
            }

            var address = new UserAddress
            {
                UserId = request.UserId,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Country = request.Country,
                AddressType = request.AddressType,
                IsDefault = request.IsDefault,
                CreatedDate = DateTime.UtcNow
            };

            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();

            return new AddCustomerAddressResponse
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                AddressType = address.AddressType,
                IsDefault = address.IsDefault
            };
        }

        public async Task<List<GetCustomerAddressesResponse>> GetCustomerAddressesAsync(int userId)
        {
            var userExists = await _context.Users.AnyAsync(x => x.IUserId == userId);
            if (!userExists)
                throw new NotFoundException("User not found");

            return await _context.UserAddresses
                .Where(x => x.UserId == userId)
                .Select(x => new GetCustomerAddressesResponse
                {
                    AddressId = x.AddressId,
                    UserId = x.UserId,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    City = x.City,
                    State = x.State,
                    PostalCode = x.PostalCode,
                    Country = x.Country,
                    AddressType = x.AddressType,
                    IsDefault = x.IsDefault
                })
                .ToListAsync();
        }

        public async Task<UpdateCustomerAddressResponse> UpdateCustomerAddressAsync(int addressId, UpdateCustomerAddressRequest request)
        {
            var address = await _context.UserAddresses.FirstOrDefaultAsync(x => x.AddressId == addressId);
            if (address == null)
                throw new NotFoundException("Address not found");

            // If setting this address as default, unset others
            if (request.IsDefault == true)
            {
                var others = await _context.UserAddresses
                    .Where(x => x.UserId == address.UserId && x.IsDefault == true && x.AddressId != addressId)
                    .ToListAsync();
                others.ForEach(a => a.IsDefault = false);
            }

            if (!string.IsNullOrWhiteSpace(request.AddressLine1)) address.AddressLine1 = request.AddressLine1;
            if (!string.IsNullOrWhiteSpace(request.AddressLine2)) address.AddressLine2 = request.AddressLine2;
            if (!string.IsNullOrWhiteSpace(request.City)) address.City = request.City;
            if (!string.IsNullOrWhiteSpace(request.State)) address.State = request.State;
            if (!string.IsNullOrWhiteSpace(request.PostalCode)) address.PostalCode = request.PostalCode;
            if (!string.IsNullOrWhiteSpace(request.Country)) address.Country = request.Country;
            if (!string.IsNullOrWhiteSpace(request.AddressType)) address.AddressType = request.AddressType;
            if (request.IsDefault.HasValue) address.IsDefault = request.IsDefault;
            address.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateCustomerAddressResponse
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                AddressType = address.AddressType,
                IsDefault = address.IsDefault
            };
        }
        public async Task<bool> DeleteCustomerAddressAsync(int addressId)
        {
            var address = await _context.UserAddresses.FirstOrDefaultAsync(x => x.AddressId == addressId);
            if (address == null)
                throw new NotFoundException("Address not found");

            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
