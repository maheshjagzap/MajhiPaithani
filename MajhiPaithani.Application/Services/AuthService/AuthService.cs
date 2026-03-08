using MajhiPaithani.Application.Interfaces.IAuthService;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Request.MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response.MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Infrastructure.Data;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.SEmail == request.sEmail);

        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            SFirstName = request.sFirstName,
            SLastName = request.sLastName,
            SEmail = request.sEmail,
            SPhoneNumber = request.sPhoneNumber,
            SPasswordHash = request.sPassword,
            IRoleId = 3,
            BIsActive = true,
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
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.SEmail == request.Email);

        if (user == null)
            throw new Exception("Invalid email or password");

        if (user.SPasswordHash != request.Password)
            throw new Exception("Invalid email or password");

        var seller = await _context.Sellers
            .FirstOrDefaultAsync(x => x.IUserId == user.IUserId);

        return new LoginResponse
        {
            UserId = user.IUserId,
            Name = user.SFirstName,
            Email = user.SEmail,
            PhoneNumber = user.SPhoneNumber,
            Role = user.IRoleId,
            //ProfileImage = user.ProfileImage,
            IsSeller = seller != null,
            SellerId = seller?.ISellerId,
            Token = "JWT_TOKEN"
        };
    }
}
