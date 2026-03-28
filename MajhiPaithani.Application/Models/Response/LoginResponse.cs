using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    namespace MajhiPaithani.Application.Models.Response
    {
        namespace MajhiPaithani.Application.Models.Response
        {
            public class LoginResponse
            {
                public int UserId { get; set; }

                public string Name { get; set; }

                public string Email { get; set; }

                public string PhoneNumber { get; set; }

                public string  Role { get; set; }
                public int  RoleId { get; set; }
                public bool? IsSellerProfileComplete { get; set; }

                public string ProfileImage { get; set; }

                public bool IsSeller { get; set; }

                public int? SellerId { get; set; }

                public string Token { get; set; }
                public string Message { get; set; }
                public UserAddressResponse? Address { get; set; }
            }

            public class UserAddressResponse
            {
                public string? AddressLine1 { get; set; }
                public string? AddressLine2 { get; set; }
                public string? City { get; set; }
                public string? State { get; set; }
                public string? PostalCode { get; set; }
                public string? Country { get; set; }
                public string? AddressType { get; set; }
            }
        }
    }
}
