using MajhiPaithani.Application.Interfaces.IAuthService;
using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add DbContext with SQL Server provider and connection string from configuration
/// Note: Ensure that the connection string "DefaultConnection" is defined in appsettings.json or user secrets
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// add services and interfaces here
/// Note: Ensure that you have implemented the AuthService class that implements the IAuthService interface
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISellerService, SellerService>();



// Build the application
/// Note: This will create the application pipeline and configure the HTTP request processing
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
