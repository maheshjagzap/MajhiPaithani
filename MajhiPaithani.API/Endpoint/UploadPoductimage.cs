/*using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MajhiPaithani.API.Endpoint
{
    public class UploadPoductimage
    {
        public static void Map(WebApplication app)
        {
            var product = app.MapGroup("/api/Dropdown");


            product.MapPost("/uploadProduct",
       static async (
           HttpRequest request,
           [FromForm] UploadProductDto productDto,
           [FromServices] IProductService service) =>
       {
           try
           {

               var product = new SaveProductDTO
               {
                   SellerId = productDto.iSellerId,
                   CategoryId = productDto.iCategoryId,
                   ProductTitle = productDto.sProductTitle,
                   Description = productDto.sDescription,
                   BasePrice = productDto.dcBasePrice,
                   Color = productDto.sColor,
                   Fabric = productDto.sFabric,
                   DesignType = productDto.sDesignType,
                   IsCustomizationAvailable = productDto.bIsCustomizationAvailable,
                   Stock = productDto.iStock
               };

               string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
               string uploadFolder = Path.Combine(root, "ProductImages");

               if (!Directory.Exists(uploadFolder))
                   Directory.CreateDirectory(uploadFolder);

               List<string> savedImages = new();

               if (productDto.ProductImages != null && productDto.ProductImages.Any())
               {
                   foreach (var file in productDto.ProductImages)
                   {
                       string fileName = $"Product_{Guid.NewGuid()}_{file.FileName}";
                       string filePath = Path.Combine(uploadFolder, fileName);

                       using var stream = new FileStream(filePath, FileMode.Create);
                       await file.CopyToAsync(stream);

                       savedImages.Add($"/ProductImages/{fileName}");
                   }
               }

               // Convert to CSV (like your docA/docB/docC)
               string productImages = string.Join(",", savedImages);

               // Save to DB
               await service.SaveProduct(product, productImages);

               return Results.Ok(new
               {
                   StatusCode = StatusCodes.Status200OK,
                   Message = "Product created successfully",
                   Images = savedImages
               });
           }
           catch (Exception ex)
           {
               return Results.Problem($"Error: {ex.Message}");
           }
       })
       .Accepts<UploadProductDto>("multipart/form-data")
       .WithName("UploadProduct")
       .WithTags("Product")
       .DisableAntiforgery();



        }
    }
}*/