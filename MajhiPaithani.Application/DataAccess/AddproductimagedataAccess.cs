using MajhiPaithani.Application.Models.Request;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.DataAccess
{

    public class AddproductimagedataAccess
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor httpContextAccessor;


        public AddproductimagedataAccess(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<string> ExecuteProductImagesAsync(List<string> fileUrls, int productid,int Taskid, int userId, int imageId)
        {
            string message = "";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // 👉 UPDATE case   imageId != 0 && productid == 0
                    if (Taskid==1)
                    {
                        using (var updateCmd = new SqlCommand(
                            "UPDATE ProductImage SET sImageUrl = @sImageUrl WHERE iImageId = @iImageId", conn))
                        {
                            updateCmd.Parameters.AddWithValue("@sImageUrl", fileUrls);
                            updateCmd.Parameters.AddWithValue("@iImageId", imageId);

                            int rowsAffected = await updateCmd.ExecuteNonQueryAsync();

                            message = rowsAffected > 0
                                ? "Image updated successfully"
                                : "No record found to update";
                        }
                    }
                    else
                    {
                        // 👉 INSERT case (existing logic)
                        using (var cmd = new SqlCommand("AddProductImageData", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            foreach (var url in fileUrls)
                            {
                                cmd.Parameters.Clear();

                                cmd.Parameters.AddWithValue("@taskid", 1);
                                cmd.Parameters.AddWithValue("@UserId", userId);
                                cmd.Parameters.AddWithValue("@iProductId", productid);
                                cmd.Parameters.AddWithValue("@sImageUrl", url);

                                using var reader = await cmd.ExecuteReaderAsync();

                                if (await reader.ReadAsync())
                                {
                                    message = reader["Message"]?.ToString();
                                }

                                reader.Close();
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }

            return message;
        }

        public async Task<string> UpdateProductImageAsync(int imageId, string newFilePath)
        {
            string message = "";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("AddProductImageData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taskid", 2);
                cmd.Parameters.AddWithValue("@imageId", imageId);
                cmd.Parameters.AddWithValue("@sImageUrl", newFilePath);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    message = reader["Message"]?.ToString();
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }
            return message;
        }

        public async Task<string> deleteProductImageAsync(int imageId)
        {
            string message = "";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("AddProductImageData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taskid", 3);
                cmd.Parameters.AddWithValue("@imageId", imageId);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                    message = reader["Message"]?.ToString();
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }
            return message;
        }

        public async Task<GetAllProductsResponseDto> GetProductImagesAsync(int userId)
        {
            var productDict = new Dictionary<int, ProductImageDto>();
            var baseUrl = GetBaseUrl();
            var summary = new InventorySummaryDto();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Step 1: Get sellerId from Sellers table by userId
            int sellerId = 0;
            using (var sellerCmd = new SqlCommand(
                "SELECT TOP 1 iSellerId FROM Sellers WHERE iUserId = @UserId AND bIsDeleted = 0", conn))
            {
                sellerCmd.Parameters.AddWithValue("@UserId", userId);
                var result = await sellerCmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                    sellerId = Convert.ToInt32(result);
            }

            // Step 2: Get inventory summary using sellerId
            using (var summaryCmd = new SqlCommand(
                "SELECT SUM(ISNULL(iStock, 0)) AS AvailableStock, SUM(ISNULL(dcBasePrice, 0)) AS InventoryValue, COUNT(*) AS ProductCount FROM Products WHERE bIsDeleted = 0 AND iSellerId = @SellerId", conn))
            {
                summaryCmd.Parameters.AddWithValue("@SellerId", sellerId);
                using var summaryReader = await summaryCmd.ExecuteReaderAsync();
                if (await summaryReader.ReadAsync())
                {
                    summary.AvailableStock = summaryReader["AvailableStock"] == DBNull.Value ? 0 : Convert.ToInt32(summaryReader["AvailableStock"]);
                    summary.InventoryValue = summaryReader["InventoryValue"] == DBNull.Value ? 0 : Convert.ToDecimal(summaryReader["InventoryValue"]);
                    summary.ProductCount = summaryReader["ProductCount"] == DBNull.Value ? 0 : Convert.ToInt32(summaryReader["ProductCount"]);
                }
            }

            // Step 3: Get product + images via SP
            using (var cmd = new SqlCommand("AddProductImageData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taskid", 0);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    int productId = Convert.ToInt32(reader["iProductId"]);

                    if (!productDict.ContainsKey(productId))
                    {
                        var product = new ProductImageDto
                        {
                            iProductId = productId,
                            iSellerId = Convert.ToInt32(reader["iSellerId"]),
                            iCategoryId = Convert.ToInt32(reader["iCategoryId"]),
                            sProductTitle = reader["sProductTitle"]?.ToString(),
                            sDescription = reader["sDescription"]?.ToString(),
                            dcBasePrice = Convert.ToDecimal(reader["dcBasePrice"]),
                            sColor = reader["sColor"]?.ToString(),
                            sFabric = reader["sFabric"]?.ToString(),
                            sDesignType = reader["sDesignType"]?.ToString(),
                            bIsCustomizationAvailable = Convert.ToBoolean(reader["bIsCustomizationAvailable"]),
                            bIsActive = Convert.ToBoolean(reader["bIsActive"]),
                            bIsDeleted = Convert.ToBoolean(reader["bIsDeleted"]),
                            ProductCreatedDate = Convert.ToDateTime(reader["ProductCreatedDate"]),
                            ProductUpdatedDate = reader["ProductUpdatedDate"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["ProductUpdatedDate"]),
                            Images = new List<ProductImageItemDto>()
                        };

                        var imageUrls = reader["sImageUrl"]?.ToString();
                        var imageIds = reader["ImageIds"]?.ToString();
                        var isPrimaryFlags = reader["IsPrimaryFlags"]?.ToString();

                        if (!string.IsNullOrEmpty(imageUrls))
                        {
                            var urlList = imageUrls.Split(',');
                            var idList = imageIds?.Split(',');
                            var primaryList = isPrimaryFlags?.Split(',');

                            for (int i = 0; i < urlList.Length; i++)
                            {
                                product.Images.Add(new ProductImageItemDto
                                {
                                    iImageId = (idList != null && i < idList.Length) ? Convert.ToInt32(idList[i]) : 0,
                                    sImageUrl = baseUrl + urlList[i],
                                    bIsPrimary = (primaryList != null && i < primaryList.Length) ? Convert.ToInt32(primaryList[i]) == 1 : (bool?)null
                                });
                            }
                        }

                        productDict[productId] = product;
                    }
                }
            }

            return new GetAllProductsResponseDto
            {
                Products = productDict.Values.ToList(),
                InventorySummary = summary
            };
        }


        public string GetBaseUrl()
        {
            var request = httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return string.Empty; 
            }

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return baseUrl;
        }


    }
}