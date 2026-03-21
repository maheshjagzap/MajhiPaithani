using MajhiPaithani.API.Endpoint;
using MajhiPaithani.API.Middleware;
using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Interfaces.IAuthService;
using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Application.Services;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

var connectionString = config.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("❌ Connection string is missing in appsettings.json.");
services.AddSingleton(connectionString);
services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
services.AddTransient<SqlConnection>(_ => new SqlConnection(connectionString));

services.AddScoped<DropdownDataAccess>();
services.AddScoped<DropdownService>();
services.AddScoped<SaveDataAccess>();
services.AddScoped<SaveSellerService>();
services.AddScoped<GetSellerDashboardService>();
services.AddScoped<GetSellerDashboardDataAccess>();
services.AddScoped<AddproductimagedataAccess>();
services.AddScoped<AddProductImageservice>();



// --- 1. REGISTER SERVICES (Dependency Injection) ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddHttpContextAccessor();
// Authentication & JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "YourDefaultFallbackKey");
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
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();

// --- 2. CONFIGURE MIDDLEWARE PIPELINE (The Order Matters!) ---

// 1st: Catch all errors and turn them into clean JSON
app.UseMiddleware<ExceptionMiddleware>();
var uploadPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "UploadedproductImages");

if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/UploadedproductImages"
});

// 2nd: Security & Swagger
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

// 3rd: Identity (Auth must be BEFORE MapControllers)
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(); // ✅ This enables serving files from wwwroot

// 4th: Routing
app.MapControllers();

//MajhiPaithani.API.Endpoints.DummyEndpoint.Map(app);
Dropdown.Map(app);
SaveData.Map(app);
GetSellerDashboard.Map(app);
UploadPoductimage.Map(app);


if (app.Environment.IsDevelopment())
{
    // When running locally (IIS Express/Kestrel)
    app.Run();
}
else
{
    // When running on the server/Docker
    app.Run("http://0.0.0.0:8080");
}
