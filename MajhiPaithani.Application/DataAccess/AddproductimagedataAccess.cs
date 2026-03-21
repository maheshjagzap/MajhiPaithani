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

        public async Task<string> ExecuteProductImagesAsync(List<string> fileUrls, int productid, int userId)
        {
            string message = "";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("AddProductImageData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

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

        public async Task<List<ProductImageDto>> GetProductImagesAsync(int userId)
        {
            var productDict = new Dictionary<int, ProductImageDto>();
            var baseUrl = GetBaseUrl();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("AddProductImageData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@taskid", 0);
                cmd.Parameters.AddWithValue("@UserId", userId);

                await conn.OpenAsync();

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

                        // ✅ Get aggregated strings
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
                                    bIsPrimary = (primaryList != null && i < primaryList.Length)? Convert.ToInt32(primaryList[i]) == 1: (bool?)null
                                });
                            }
                        }

                        productDict[productId] = product;
                    }
                }
            }

            return productDict.Values.ToList();
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