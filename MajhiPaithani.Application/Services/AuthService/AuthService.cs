using MajhiPaithani.Application.Interfaces.IAuthService;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Request.MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Domain.Exceptions;
using MajhiPaithani.Infrastructure.Data;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthService(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
     
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.SEmail == request.sEmail);

        if (existingUser != null)
        {
            throw new ConflictException("User already exists");
        }
        bool IsSellerProfileComplete;
        if (request.RoleId ==2)
        {
            IsSellerProfileComplete = false;
        }
        else
        {
            IsSellerProfileComplete = true;
        }
            var user = new User
            {
                SFirstName = request.sFirstName,
                SLastName = request.sLastName,
                SEmail = request.sEmail,
                SPhoneNumber = request.sPhoneNumber,
                SPasswordHash = request.sPassword,
                IRoleId = request.RoleId,
                BIsActive = true,
                IsSellerProfileComplete = IsSellerProfileComplete,
                DCreatedDate = DateTime.UtcNow
            };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RegisterResponse
        {
            iUserId = user.IUserId,
            sEmail = user.SEmail,
            Message = "User registered successfully"
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        Seller? seller = null;

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.SEmail == request.EmailOrPhone 
                                || x.SPhoneNumber == request.EmailOrPhone);

        if (user == null)
            throw new UnauthorizedException("Invalid credentials");

        if (user.SPasswordHash != request.Password)
            throw new UnauthorizedException("Invalid credentials");

        if (user.IRoleId == 2)
        {
            seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.IUserId == user.IUserId);
        }

        var role = user.IRoleId == 1 ? "Admin" :
                   user.IRoleId == 2 ? "Seller" :
                   user.IRoleId == 3 ? "Customer" : "";

        // Generate JWT Token
        var token = _jwtTokenService.GenerateToken(
            user.IUserId,
            user.SEmail,
            role
        );

        return new LoginResponse
        {
            UserId = user.IUserId,
            Name = user.SFirstName,
            Email = user.SEmail,
            PhoneNumber = user.SPhoneNumber,
            Role = role,
            IsSellerProfileComplete= user.IsSellerProfileComplete,
            RoleId = user.IRoleId,
            IsSeller = seller != null,
            SellerId = seller?.ISellerId,
            Token = token,
            Message = "Login successful"
        };
    }
}
