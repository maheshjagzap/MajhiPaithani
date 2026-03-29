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
    private readonly IEmailService _emailService;

    // In-memory OTP store: email -> (otp, expiry)
    private static readonly Dictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();

    public AuthService(ApplicationDbContext context, IJwtTokenService jwtTokenService, IEmailService emailService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _emailService = emailService;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
     
        var emailExists = await _context.Users.AnyAsync(x => x.SEmail == request.sEmail);

        if (emailExists)
            throw new ConflictException("This email already exists");

        var mobileExists = await _context.Users.AnyAsync(x => x.SPhoneNumber == request.sPhoneNumber);

        if (mobileExists)
            throw new ConflictException("This mobile number already exists");

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
        UserAddress? userAddress = null;

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
        else if (user.IRoleId == 3)
        {
            userAddress = await _context.UserAddresses
                .FirstOrDefaultAsync(x => x.UserId == user.IUserId && x.IsDefault == true);
        }

        var role = user.IRoleId == 1 ? "Admin" : user.IRoleId == 2 ? "Seller" : user.IRoleId == 3 ? "Customer" : "";

        var cartItemCount = 0;
        if (user.IRoleId == 3)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.IUserId == user.IUserId && c.Status == "Active");
            if (cart != null)
                cartItemCount = await _context.CartItems.CountAsync(ci => ci.ICartId == cart.CartId);
        }

        // Generate JWT Token
        var token = _jwtTokenService.GenerateToken(
            user.IUserId,
            user.SEmail,
            role,
            user.SFirstName + " " + user.SLastName,
            user.SPhoneNumber
        );

        return new LoginResponse
        {
            UserId = user.IUserId,
            Name = user.SFirstName + " " + user.SLastName,
            Email = user.SEmail,
            PhoneNumber = user.SPhoneNumber,
            Role = role,
            IsSellerProfileComplete = user.IsSellerProfileComplete ?? false,
            RoleId = user.IRoleId ?? 0,
            IsSeller = seller != null,
            SellerId = seller?.ISellerId,
            Token = token,
            Message = "Login successful",
            CartItemCount = cartItemCount,
            Address = userAddress == null ? null : new UserAddressResponse
            {
                AddressId = userAddress.AddressId,
                AddressLine1 = userAddress.AddressLine1,
                AddressLine2 = userAddress.AddressLine2,
                City = userAddress.City,
                State = userAddress.State,
                PostalCode = userAddress.PostalCode,
                Country = userAddress.Country,
                AddressType = userAddress.AddressType
            }
        };
    }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.IUserId == request.UserId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Check old password (plain text for testing)
        if (user.SPasswordHash != request.oldPassword)
        {
            throw new UnauthorizedException("Old password is incorrect");
        }

        // Update new password (plain text for now)
        user.SPasswordHash = request.NewPassword;
        user.DUpdatedDate = DateTime.UtcNow;

        // Save changes
        await _context.SaveChangesAsync();

        return new ChangePasswordResponse
        {
            Message = "Password changed successfully"
        };
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.SEmail == request.Email);
        if (user == null)
            throw new Exception("No account found with this email");

        var otp = new Random().Next(100000, 999999).ToString();
        _otpStore[request.Email] = (otp, DateTime.UtcNow.AddMinutes(10));

        await _emailService.SendEmailAsync(
            request.Email,
            "Password Reset OTP - MajhiPaithani",
            $"Your OTP for password reset is: {otp}. It is valid for 10 minutes."
        );

        return new ForgotPasswordResponse { Message = "OTP sent to your email" };
    }

    public Task<ForgotPasswordResponse> VerifyOtpAsync(VerifyOtpRequest request)
    {
        if (!_otpStore.TryGetValue(request.Email, out var entry) ||
            entry.Otp != request.Otp ||
            entry.Expiry < DateTime.UtcNow)
            throw new UnauthorizedException("Invalid or expired OTP");

        return Task.FromResult(new ForgotPasswordResponse { Message = "OTP verified successfully" });
    }

    public async Task<ForgotPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (!_otpStore.TryGetValue(request.Email, out var entry) ||
            entry.Otp != request.Otp ||
            entry.Expiry < DateTime.UtcNow)
            throw new UnauthorizedException("Invalid or expired OTP");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.SEmail == request.Email);
        if (user == null)
            throw new Exception("User not found");

        user.SPasswordHash = request.NewPassword;
        user.DUpdatedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _otpStore.Remove(request.Email);

        return new ForgotPasswordResponse { Message = "Password reset successfully" };
    }
}

