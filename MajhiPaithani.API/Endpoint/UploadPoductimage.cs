using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Services;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace MajhiPaithani.API.Endpoint
{
    public class UploadPoductimage
    {
        public static void Map(WebApplication app)
        {
            var product = app.MapGroup("/api/Product");


            product.MapPost("api/upload/images", async (
    [FromForm] int? ProdcutId,
    [FromForm] int?  userId,
    [FromForm] IFormFileCollection Files,
    AddProductImageservice service,
    HttpContext context) =>
            {
                try
                {
                    if (Files == null || Files.Count == 0)
                        return Results.BadRequest("No files uploaded.");

                    var fileUrls = new List<string>();

                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadFolderPath = Path.Combine(wwwRootPath, "UploadedproductImages");

                    if (!Directory.Exists(uploadFolderPath))
                        Directory.CreateDirectory(uploadFolderPath);

                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                            using var stream = new FileStream(filePath, FileMode.Create);
                            await file.CopyToAsync(stream);

                            var relativePath = $"/UploadedproductImages/{uniqueFileName}";

                            fileUrls.Add(relativePath);
                        }
                    }

                    await service.SaveProductImagesAsync(fileUrls, ProdcutId ?? 0,userId ?? 0);

                    return Results.Ok(new
                    {
                        StatusCode = 200,
                        Message = "Images uploaded successfully",
                        Files = fileUrls
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error: {ex.Message}");
                }
            })
.WithName("UploaProductImmage")
.WithTags("Prodcuts")
.DisableAntiforgery();


            product.MapGet("/GetAllProductdata", async (
    int userId,
    AddProductImageservice service) =>
            {
                try
                {
                    var data = await service.GetProductImagesAsync(userId);

                    return Results.Ok(new
                    {
                        StatusCode = 200,
                        Message = "Product data fetched successfully",
                        Data = data
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error: {ex.Message}");
                }
            })
.WithName("GetProductImages")
.WithTags("Prodcuts");

        }

    }
}

