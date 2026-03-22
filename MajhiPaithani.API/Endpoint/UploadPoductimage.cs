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
    [FromForm] int? userId,
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

                    await service.SaveProductImagesAsync(fileUrls, ProdcutId ?? 0, userId ?? 0);

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


            product.MapPut("/updateproductimage", async (
    int imageId,
   [FromQuery] string? fileUrl,
    IFormFile newFile,
    AddProductImageservice service) =>
            {
                try
                {
                    if (newFile == null || newFile.Length == 0)
                        return Results.BadRequest("File is required");

                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadFolderPath = Path.Combine(wwwRootPath, "UploadedproductImages");

                    if (!Directory.Exists(uploadFolderPath))
                        Directory.CreateDirectory(uploadFolderPath);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        var oldFilePath = Path.Combine(wwwRootPath, fileUrl.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                            File.Delete(oldFilePath);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(newFile.FileName)}";
                    var newPath = Path.Combine(uploadFolderPath, uniqueFileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                        await newFile.CopyToAsync(stream);

                    var newFilePath = $"/UploadedproductImages/{uniqueFileName}";

                    await service.UpdateProductImageAsync(imageId, newFilePath);

                    
                    return Results.Ok(new
                    {
                        StatusCode = 200,
                        Message = "Image updated successfully",
                        File = newFilePath
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Error: {ex.Message}");
                }
            })
.WithName("UpdateProductImage")
.WithTags("Prodcuts")
.Accepts<IFormFile>("multipart/form-data")
.DisableAntiforgery();

//            product.MapPut("/update/product-image", async(
//  [FromQuery]  int imageId,
//   [FromQuery] string? fileUrl,
//AddProductImageservice service) =>
//            {
//                try
//                {


//                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");


//                    if (!string.IsNullOrEmpty(fileUrl))
//                    {
//                        var oldFilePath = Path.Combine(wwwRootPath, fileUrl.TrimStart('/'));
//                        if (File.Exists(oldFilePath))
//                            File.Delete(oldFilePath);
//                    }



//                    await service.deleteProductImageAsync(imageId);

//                    return Results.Ok(new
//                    {
//                        StatusCode = 200,
//                        Message = "Image deleted successfully"

//                    });
//                }
//                catch (Exception ex)
//                {
//                    return Results.Problem($"Error: {ex.Message}");
//                }
//            })
//.WithName("deleteProductimmage")
//.WithTags("Products")
//.Accepts<IFormFile>("multipart/form-data")
//.DisableAntiforgery();


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

