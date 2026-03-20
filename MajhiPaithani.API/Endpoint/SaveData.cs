using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MajhiPaithani.API.Endpoint
{
    public class SaveData
    {
        public static void Map(WebApplication app)
        {
            var savedata = app.MapGroup("/api/savedata");

            savedata.MapPost("/SaveSeller", async (
                [FromBody] SellerDto dto, [FromHeader(Name = "UserId")] int? userId,[FromHeader(Name = "RoleId")] int? roleId,

                [FromServices] SaveSellerService service) =>
            {
                try
                {
                    var message = await service.SaveSellerAsync(dto,userId,roleId);

                    return Results.Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = message,
                        Data = dto
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("SaveSeller")
            .WithTags("SaveData");


            savedata.MapPost("/Savesellerbankdeatils", async ([FromBody] BankDto dto, [FromHeader(Name = "UserId")] int? userId,
    [FromHeader(Name = "RoleId")] int? roleId, [FromServices] SaveSellerService service) =>
            {
                try
                {
                    var message = await service.updatebankdeatils(dto,userId ,roleId);

                    return Results.Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = message,
                        Data = dto
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("SaveSellerbankdeatils")
            .WithTags("SaveData");


            savedata.MapPost("/AddProductdeatils", async ([FromBody] ProductDto dto, [FromHeader(Name = "UserId")] int? userId,
            [FromHeader(Name = "RoleId")] int? roleId, [FromServices] SaveSellerService service) =>
            {
                try
                {
                    var message = await service.AddProdudctinfoasync(dto, userId, roleId);

                    return Results.Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = message,
                        Data = dto
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("Addprodcutdeatils")
            .WithTags("SaveData");





        }
    }
}