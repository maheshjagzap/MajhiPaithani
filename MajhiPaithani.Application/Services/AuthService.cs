using MajhiPaithani.Application.Interfaces;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Infrastructure.Data;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;

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
            SEmail= request.sEmail,
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
}