using MajhiPaithani.Application.Interfaces.IAuthService;
using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddAuthentication("JwtBearer")
.AddJwtBearer("JwtBearer", options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI();

// Middleware
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Majhi Paithani API is running 🚀");
app.MapControllers();

// Render port binding
app.Run("http://0.0.0.0:8080");