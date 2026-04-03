using MajhiPaithani.Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace MajhiPaithani.API.Endpoint
{     
        public static class Dropdown
        {
            public static void Map(WebApplication app)
            {
                var dropdown = app.MapGroup("/api/Dropdown");

                dropdown.MapGet("/", async ([FromQuery] int taskId, DropdownService service) =>
                {
                    try
                    {
                        var data = await service.GetDropdownListAsync(taskId);

                        return Results.Ok(new
                        {
                            StatusCode = StatusCodes.Status200OK,
                            Message = data.Any() ? "Dropdown data fetched successfully" : "No data found",
                            Data = data
                        });
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(detail: $"An error occurred: {ex.Message}");
                    }
                })
                .WithName("GetDropdown")
                .WithTags("Dropdown");//
            }
        }
    
}
