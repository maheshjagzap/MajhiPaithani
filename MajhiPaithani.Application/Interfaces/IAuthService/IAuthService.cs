using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Request.MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Interfaces.IAuthService
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);


    }
}
