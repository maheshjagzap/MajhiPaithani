using MajhiPaithani.API.Endpoint;
using MajhiPaithani.API.Hubs;
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
        // SignalR sends token via query string, not Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/chat"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
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


// SignalR — enable detailed errors so hub exceptions are visible on client
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Two CORS policies:
// "AllowAll"     — used by all REST API endpoints (allows any origin)
// "SignalRPolicy" — used only by SignalR hub (requires specific origins + credentials)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

    options.AddPolicy("SignalRPolicy", policy =>
        policy.WithOrigins(
                "https://localhost:7006",
                "http://localhost:7006",
                "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:4200",
                "http://127.0.0.1:5500",
                "https://mazipaithaniadmin.onrender.com"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
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
app.MapHub<ChatHub>("/hubs/chat").RequireCors("SignalRPolicy");

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
    app.Run("");
}
